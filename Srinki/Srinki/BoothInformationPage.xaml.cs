using Srinki.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Srinki
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoothInformationPage : ContentPage
    {
        ListView listView;
        BoothInformation boothInformation = new BoothInformation();
        int currentBoothNumberDisplayed;
        public BoothInformationPage(int boothNumber = 0)
        {
            InitializeComponent();

            var boothNumberInput = new Entry
            {
                Placeholder = "Booth Number",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Blue,
                Keyboard = Keyboard.Numeric,
                FontSize = 25
            };
            boothNumberInput.Completed += BoothNumberInput_Completed;
            currentBoothNumberDisplayed = boothNumber;
            listView = new ListView
            {
                ItemsSource = boothNumber > 0 ?
                              boothInformation.GetBoothInformationDisplayItems(boothNumber) :
                              new List<DisplayItem>(), //Create a empty list
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, new Binding("Text"));
                    cell.SetBinding(TextCell.DetailProperty, new Binding("Detail"));
                    cell.TextColor = Color.Red;
                    return cell;
                }),
                SeparatorColor = Color.Black,
            };

            listView.ItemSelected += ListView_ItemSelected;

            var shareButton = new Button
            {
                Text = "Share Details",
                TextColor = Color.Blue,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            shareButton.Clicked += ShareButton_Clicked;

            var agentDetails = new Button
            {
                Text = "Agent Details",
                TextColor = Color.Blue,
                HorizontalOptions =  LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };

            // Build the page.
            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    boothNumberInput,
                    listView,
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            shareButton,
                            agentDetails
                        }                        
                    },                    
                },
            };
        }

        async private void ShareButton_Clicked(object sender, EventArgs e)
        {            
            try
            { 
                var items = boothInformation.GetBoothInformationDisplayItems(currentBoothNumberDisplayed);
                string str = "";
                foreach(var item in items)
                {
                    str += item.Text + ":" + item.Detail + "\n";
                }
                IntentService.SendData(str);
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error!", ex.Message, "Ok");
            }
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var item = (DisplayItem)e.SelectedItem;
            ((ListView)sender).SelectedItem = null;
            await DisplayAlert(item.Text, item.Detail, "Ok");
        }

        async private void BoothNumberInput_Completed(object sender, EventArgs e)
        {
            int boothNumber = 0;
            try
            {
                Entry en = (Entry)sender;
                boothNumber = Int32.Parse(en.Text);
                listView.ItemsSource = boothInformation.GetBoothInformationDisplayItems(boothNumber);
                currentBoothNumberDisplayed = boothNumber;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Booth Information not loaded or BoothNumber " + boothNumber + " is invalid\n" + ex.Message, "Ok");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed(); ;
        }
    }
}
