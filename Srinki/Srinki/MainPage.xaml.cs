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
        Button updateData, boothInformation, agentInformation, Contacts, notification, stats;
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
                IsEnabled = false
            };
            boothInformation.Clicked += BoothInformation_Clicked;

            agentInformation = new Button
            {
                Text = "Agent Information",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,
                IsEnabled = false
            };

            Contacts = new Button
            {
                Text = "Frequent Contacts",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,
                IsEnabled = false
            };

            updateData = new Button
            {
                Text = "Update Data",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,                
            };

            notification = new Button
            {
                Text = "4 new notification",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,
                IsEnabled = false
            };

            stats = new Button
            {
                Text = "Stats",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,
                IsEnabled = false
            };

            grid.Children.Add(boothInformation, 0, 0);
            grid.Children.Add(agentInformation, 0, 1);
            grid.Children.Add(Contacts, 1, 0);
            grid.Children.Add(updateData, 1, 1);

            grid.Children.Add(notification, 0, 2);
            grid.Children.Add(stats, 1, 2);

            stats.Clicked += Stats_Clicked;
            updateData.Clicked += UpdateData_Clicked;

            if(DataService.getDataService().DataStatus)
            {
                updateData.IsEnabled = true;
                boothInformation.IsEnabled = true;
                agentInformation.IsEnabled = true;
                stats.IsEnabled = true;
            }
            
            this.Content = grid;
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
                    updateData.Text = updated == false ? "Failed to Update Data. Try again?" : "Update Data";
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
