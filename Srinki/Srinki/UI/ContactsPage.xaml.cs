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
	public partial class ContactsPage : ContentPage
	{
        ListView listView;
        Picker wardPicker;
        List<int> wardList;
        public ContactsPage ()
		{
            wardPicker = new Picker { Title = "Choose Ward Number" };
            wardPicker.SelectedIndexChanged += WardPicker_SelectedIndexChanged; ;

            wardList = DataService.getDataService().getAllWardNumbers();
            wardList.ForEach(x => { wardPicker.Items.Add("Ward Number" + x.ToString()); });

            listView = new ListView
            {
                ItemsSource = new List<DisplayItem>(),
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, new Binding("Text"));
                    cell.SetBinding(TextCell.DetailProperty, new Binding("Detail"));
                    cell.TextColor = Color.Red;
                    cell.Height = 50;
                    return cell;
                }),
                SeparatorColor = Color.Black,
            };
            listView.SeparatorColor = Color.Blue;
            listView.ItemSelected += ListView_ItemSelected;

            // Build the page.
            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {    
                    wardPicker,
                    listView
                },
            };
        }

        private void WardPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int wardNumber = wardList[picker.SelectedIndex];
            var contactList = DataService.getDataService().getContactsInformationByWard(wardNumber);
            listView.ItemsSource = contactList;            
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var item = (DisplayItem)e.SelectedItem;
            ((ListView)sender).SelectedItem = null;
            if (item.phoneNumber == "")
            {
                await DisplayAlert(item.Text, item.Detail, "Ok");
            }
            else
            {
                var answer = await DisplayAlert(item.Text, item.Detail, "Ok", "Call");
                if (!answer)
                {
                    IntentService.Call(item.phoneNumber);
                }
            }
        }
    }
}