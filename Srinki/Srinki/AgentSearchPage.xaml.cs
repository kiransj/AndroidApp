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
        Switch searchType;
        Entry searchText;
        bool searchByName = true;
        public AgentSearchPage()
        {
            InitializeComponent();

            searchText = new Entry
            {
                Placeholder = "Search by Name",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            searchText.Completed += searchText_Completed;

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
            listView.ItemSelected += ListView_ItemSelected;

            searchType = new Switch
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,                

            };
            searchType.Toggled += SearchType_Toggled;

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
                            searchType
                        }
                    },
                    listView,
                },
            };
        }

        private void SearchType_Toggled(object sender, ToggledEventArgs e)
        {
            if(searchByName)
            {
                searchText.Placeholder = "Search by Agent Detail";
            }
            else
            {

                searchText.Placeholder = "Search by Name";
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        async private void searchText_Completed(object sender, EventArgs e)
        {
            string name = "";
            try
            {
                Entry en = (Entry)sender;
                name = en.Text;
                listView.ItemsSource = DataService.getDataService().GetAgentInformationDisplayItemsByName(name);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Agent Information not found for " + name + "\n" + ex.Message, "Ok");
            }
        }
    }
}
