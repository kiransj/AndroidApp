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

                    return cell;
                }),
                SeparatorColor = Color.Black,
            };
            listViewAddressSearch.SeparatorColor = Color.Blue;

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

                    return cell;
                }),
                SeparatorColor = Color.Black,
            };
            listViewWardSearch.SeparatorColor = Color.Blue;

            var page1 = 
            

            wardPicker = new Picker
            {
                Title = "Choose Ward Number",                
            };

            wardList = DataService.getDataService().getAllWardNumbers();
            foreach (var ward in )
            {
                wardPicker.Items.Add(ward.ToString());
            }
                   

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
    }
}
