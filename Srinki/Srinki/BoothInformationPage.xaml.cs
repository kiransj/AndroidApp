﻿using Srinki.DataModel;
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
        public BoothInformationPage(int boothNumber = 1)
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
            
            listView = new ListView
            {                
                ItemsSource = boothInformation.GetBoothInformationDisplayItems(boothNumber),
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
                Text = "Share",
                TextColor = Color.Blue
            };            

            // Build the page.
            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,                
                Children =
                {
                    boothNumberInput,
                    listView,
                    shareButton
                }                
            };
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
            int boothNumber = 0 ;
            try
            {
                Entry en = (Entry)sender;
                boothNumber = Int32.Parse(en.Text);
                listView.ItemsSource = boothInformation.GetBoothInformationDisplayItems(boothNumber); ;
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", "Booth Information not loaded or BoothNumber " + boothNumber + " is invalid\n" + ex.Message, "Ok");
            }
        }
    }
}
