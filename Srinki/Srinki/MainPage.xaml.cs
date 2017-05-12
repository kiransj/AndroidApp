using Android.App;
using Srinki.DataModel;
using Srinki.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Srinki
{
    public partial class MainPage : ContentPage
    {        
        Button updateData, boothInformation, agentInformation, boothSearch, contacts, stats;
        public MainPage()
        {
            InitializeComponent();

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            boothInformation = new Button
            {
                Text = "Booth Information",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,
                IsEnabled = false,
                Margin = 1,
            };
            boothInformation.Clicked += BoothInformation_Clicked;

            agentInformation = new Button
            {
                Text = "Agent Information",
                Margin = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,                
                IsEnabled = false
            };
            agentInformation.Clicked += AgentInformation_Clicked;

            boothSearch = new Button
            {
                Text = "Booth Search",
                Margin = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,                                
            };
            boothSearch.Clicked += BoothSearch_Clicked;

            updateData = new Button
            {
                Text = "Update Data\nLast updated on " + Properties.lastUpdatedTime,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,
                Margin = 1,
            };
            updateData.Clicked += UpdateData_Clicked;

            contacts = new Button
            {
                Text = "Contacts",
                Margin = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Red,                
                IsEnabled = false
            };

            stats = new Button
            {
                Text = "Stats",
                Margin = 1,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,                
                IsEnabled = false                
            };
            stats.Clicked += Stats_Clicked;

            grid.Children.Add(boothInformation, 0, 0);
            grid.Children.Add(agentInformation, 0, 1);
            grid.Children.Add(boothSearch, 1, 0);
            grid.Children.Add(updateData, 1, 1);

            grid.Children.Add(contacts, 0, 2);
            grid.Children.Add(stats, 1, 2);

#if false
            agentInformation.Image = "voting.png";
            stats.Image = "stats";
            Contacts.Image = "Contacts";
            notification.Image = "notification";
#endif
                        
            if(DataService.getDataService().DataStatus)
            {
                updateData.IsEnabled = true;
                boothInformation.IsEnabled = true;
                agentInformation.IsEnabled = true;
                stats.IsEnabled = true;
            }
            
            this.Content = grid;            
        }

        async private void BoothSearch_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new BoothSearchPage());
        }

        async private void AgentInformation_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new AgentSearchPage());
        }

        private void Stats_Clicked(object sender, EventArgs e)
        {
            
        }

        private void UpdateData_Clicked(object sender, EventArgs e)
        {                            
            updateData.IsEnabled = false;
            updateData.Text = "Updating data from internet";
            updateData.TextColor = Color.Blue; 
            ThreadPool.QueueUserWorkItem(o => updateInformation());
        }

        private void updateInformation()
        {            
            bool updated = false;
            try
            {
                DataService.getDataService().UpdateData();
                updated = true;
            }
            catch(Exception ex)
            {                
                Device.BeginInvokeOnMainThread(async () => 
                {
                    await DisplayAlert("Error!", "Failed to update data from internet\n" + ex.Message, "Ok");                                        
                });
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() => {
                    updateData.IsEnabled = true;
                    boothInformation.IsEnabled = true;
                    agentInformation.IsEnabled = true;
                    updateData.Text = updated == false ? "Failed to Update Data. Try again?" : "Update Data\nLast Updated on " + Properties.lastUpdatedTime;
                    updateData.TextColor = (updated == true) ? Color.Green : Color.Red;
                });
            }
        }

        async private void BoothInformation_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushModalAsync(new BoothInformationPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}
