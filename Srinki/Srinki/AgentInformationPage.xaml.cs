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
	public partial class AgentInformationPage : ContentPage
	{                
        public AgentInformationPage(int boothNumber)
        {
            InitializeComponent();

            MenuItem callAgent = new MenuItem { Text = "Call" };
            callAgent.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            callAgent.Clicked += callAgent_Clicked1;

            MenuItem shareAgentInformation = new MenuItem { Text = "Share" };
            shareAgentInformation.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            shareAgentInformation.Clicked += ShareAgent_Clicked1;

            ListView listView = new ListView
            {
                ItemsSource = DataService.getDataService().GetAgentInformationDisplayItems(boothNumber),
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, new Binding("Text"));
                    cell.SetBinding(TextCell.DetailProperty, new Binding("Detail"));
                    cell.TextColor = Color.Red;
                    cell.ContextActions.Add(callAgent);
                    cell.ContextActions.Add(shareAgentInformation);                    
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
                    new Label {Text = "Agent details for Booth Number " + boothNumber, FontSize = 20},
                    listView
                },
            };

        }

        private void callAgent_Clicked1(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = (DisplayItem)mi.CommandParameter;            
            IntentService.Call(item.phoneNumber);
        }

        async private void ShareAgent_Clicked1(object sender, EventArgs e)
        {            
            await DisplayAlert("Share", "Clicked!", "Ok");
        }

        async private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedItem == null) return;
            DisplayItem di = (DisplayItem)lv.SelectedItem;
            lv.SelectedItem = null;
            await DisplayAlert(di.Text, di.Detail, "ok_1");            
        }        

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed(); ;
        }
    }
}
