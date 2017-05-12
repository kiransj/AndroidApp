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
	public partial class LoginPage : ContentPage
	{
        static bool loggedIn = false;
        public LoginPage ()
		{            
			InitializeComponent ();
            var pinCode = new Entry
            {
                Placeholder = "Enter Pin Code",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Red,
                Keyboard = Keyboard.Numeric,
                FontSize = 25,
                IsPassword = true
            };
            pinCode.Completed += PinCode_Completed;            

            // Build the page.
            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    pinCode
                }
            };

            this.Appearing += (object sender, EventArgs e) => pinCode.Focus();
            BackgroundImage = "voting";
        }


        async private void PinCode_Completed(object sender, EventArgs e)
        {
            Entry en = (Entry)sender;            
            switch(en.Text)
            {                
                case "3411":
                    loggedIn = true;
                    await this.Navigation.PushModalAsync(new MainPage());
                    return;
                default:
                    return;
            }
        }

        public static bool loginStatus()
        {
            return loggedIn;
        }
    }
}
