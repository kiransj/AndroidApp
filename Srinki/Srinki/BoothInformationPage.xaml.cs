using Srinki.DataModel;
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
        public BoothInformationPage()
        {
            InitializeComponent();

            var boothNumberInput = new Entry
            {
                Placeholder = "Enter Booth Number",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Blue,
                Keyboard = Keyboard.Numeric
            };

            var boothInformation = (new BoothInformation()).GetBoothInformationDisplayItems(1);
            var listView = new ListView
            {
                ItemsSource = boothInformation,
                HasUnevenRows = true,
              /*  IsGroupingEnabled = true,
                GroupShortNameBinding = new Binding("Key"),
                GroupDisplayBinding = new Binding("Key"),*/
                ItemTemplate = new DataTemplate(() =>
                {
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, new Binding("Text"));
                    cell.SetBinding(TextCell.DetailProperty, new Binding("Detail"));
                    cell.TextColor = Color.Red;
                    return cell;
                })
            };
        
            // Build the page.
            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    boothNumberInput,
                    listView
                }
            };
        }
    }
}
