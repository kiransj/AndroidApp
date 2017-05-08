using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Srinki
{
    public partial class MainPage : ContentPage
    {

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

            var updateData = new Button
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


            this.Content = grid;
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
