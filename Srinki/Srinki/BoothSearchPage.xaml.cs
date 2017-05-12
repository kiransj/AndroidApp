﻿using Srinki.services;
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
    public partial class BoothSearchPage : TabbedPage
    {
        ListView listViewAddressSearch;
        ListView listViewWardSearch;
        List<int> wardList;
        Picker wardPicker;
        public BoothSearchPage()
        {
            InitializeComponent();
            BarBackgroundColor = Color.Blue;

            MenuItem boothInformation = new MenuItem { Text = "Booth Information" };
            boothInformation.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            boothInformation.Clicked += boothInformation_Clicked1;

            listViewAddressSearch = new ListView
            {
                ItemsSource = new List<DisplayItem>(), //Create a empty list
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, new Binding("Text"));
                    cell.SetBinding(TextCell.DetailProperty, new Binding("Detail"));
                    cell.TextColor = Color.Red;
                    cell.ContextActions.Add(boothInformation);                    

                    return cell;
                }),
                SeparatorColor = Color.Black,
            };
            listViewAddressSearch.SeparatorColor = Color.Blue;
            listViewAddressSearch.ItemSelected += ListViewAddressSearch_ItemSelected;

            listViewWardSearch = new ListView
            {
                ItemsSource = new List<DisplayItem>(), //Create a empty list
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, new Binding("Text"));
                    cell.SetBinding(TextCell.DetailProperty, new Binding("Detail"));
                    cell.TextColor = Color.Red;
                    cell.ContextActions.Add(boothInformation);                    

                    return cell;
                }),
                SeparatorColor = Color.Black,
            };
            listViewWardSearch.SeparatorColor = Color.Blue;
            listViewWardSearch.ItemSelected += ListViewAddressSearch_ItemSelected;

            wardPicker = new Picker { Title = "Choose Ward Number" };
            wardPicker.SelectedIndexChanged += WardPicker_SelectedIndexChanged;

            wardList = DataService.getDataService().getAllWardNumbers();            
            wardList.ForEach(x =>  { wardPicker.Items.Add("Booths in " + x.ToString()); });

            this.Children.Add(new ContentPage
            {
                Title = "Booth Search by Address",
                Content = new StackLayout
                {
                    Children =
                    {
                        new Entry
                        {
                            Placeholder = "Enter booth Address",
                            FontSize = 30,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Keyboard = Keyboard.Text
                        },
                        listViewAddressSearch
                    }
                }
            });

            this.Children.Add(new ContentPage
            {
                Title = "Booth Search by ward",
                Content = new StackLayout
                {
                    Children =
                    {
                        wardPicker,
                        listViewWardSearch
                    }
                },
            });
        }

        async private void ListViewAddressSearch_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListView lv = (ListView)sender;
            if (lv.SelectedItem == null) return;
            DisplayItem item = (DisplayItem)lv.SelectedItem;
            lv.SelectedItem = null;            
            await DisplayAlert(item.Text, item.Detail, "Ok");
        }

        private void WardPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int wardNumber = wardList[picker.SelectedIndex];
            listViewWardSearch.ItemsSource = DataService.getDataService().getBoothInformationDisplayItemByWard(wardNumber);            
        }

        async private void boothInformation_Clicked1(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = (DisplayItem)mi.CommandParameter;
            await this.Navigation.PushModalAsync(new BoothInformationPage(item.boothNumber));
        }

    }
}
