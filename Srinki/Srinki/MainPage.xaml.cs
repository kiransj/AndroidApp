using Android.App;
using Srinki.DataModel;
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
        Button updateData;
        public MainPage()
        {
            InitializeComponent();

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var boothInformation = new Button
            {
                Text = "Booth Information",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green
            };
            boothInformation.Clicked += BoothInformation_Clicked;

            var agentInformation = new Button
            {
                Text = "Agent Information",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green
            };

            var Contacts = new Button
            {
                Text = "Frequent Contacts",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green
            };

            updateData = new Button
            {
                Text = "Update Data",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green
            };

            var notification = new Button
            {
                Text = "notifications",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green
            };

            var settings = new Button
            {
                Text = "Settings",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,                
            };

            grid.Children.Add(boothInformation, 0, 0);
            grid.Children.Add(agentInformation, 0, 1);
            grid.Children.Add(Contacts, 1, 0);
            grid.Children.Add(updateData, 1, 1);

            grid.Children.Add(notification, 0, 2);
            grid.Children.Add(settings, 1, 2);
            
            updateData.Clicked += UpdateData_Clicked;
            // Try updating data
            UpdateData_Clicked(null, null);
            
            this.Content = grid;
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
                (new RoadInformation()).UpdateInformation();
                (new BoothInformation()).UpdateInformation();
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
