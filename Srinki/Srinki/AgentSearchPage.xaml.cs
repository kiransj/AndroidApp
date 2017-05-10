using Srinki.services;
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
    public partial class AgentSearchPage : ContentPage
    {
        ListView listView;
        Label label;
        public AgentSearchPage()
        {
            InitializeComponent();

            var searchText = new Entry
            {
                Placeholder = "Search by Name",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Blue
            };
            searchText.TextChanged += searchText_Completed;

            label = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Color.Blue
            };

            listView = new ListView
            {
                ItemsSource = new List<DisplayItem>(), //Create a empty list
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
            listView.ItemTapped += ListView_ItemTapped;

            // Build the page.
            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            searchText,
                            label,
                        }
                    },
                    listView,
                },
            };

            this.Appearing += (object sender, EventArgs e) => searchText.Focus();

            //BackgroundImage = "stats";
        }


        async private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            if (e.Item == null) return;
            DisplayItem item = (DisplayItem)e.Item;

            listView.SelectedItem = null;
            string action = await DisplayActionSheet("Actions", "Cancel", null, "Call", "Share", "Info");
            switch(action)
            {
                case "Cancel": return;
                case "Call": IntentService.Call(item.phoneNumber); return;
                case "Share":
                    {
                        var boothInformation = DataService.getDataService().GetBoothInformation(item.boothNumber);
                        var details = item.Detail + "\nBooth Address " + boothInformation.address + "\nPopulation: " + boothInformation.population;
                        IntentService.SendData(details); return;
                    }
                case "Info":
                    {
                        var boothInformation = DataService.getDataService().GetBoothInformation(item.boothNumber);
                        var details = item.Detail + "\nBooth Address " + boothInformation.address + "\nPopulation: " + boothInformation.population;
                        await DisplayAlert(item.Text, details, "Ok");
                    }
                    return;
                default:
                    return;
            }
        }


        async private void searchText_Completed(object sender, EventArgs e)
        {
            string name = "";
            try
            {
                Entry en = (Entry)sender;
                name = en.Text;                                
                var result = DataService.getDataService().GetAgentInformationDisplayItemsByName(name);
                listView.ItemsSource = result;
                label.Text = string.Format("{0} Agent found", result.Count);
            }
            catch (Exception ex)
            {
                
                await DisplayAlert("Error", "Agent Information not found for " + name + "\n" + ex.Message, "Ok");
            }
        }
    }
}
