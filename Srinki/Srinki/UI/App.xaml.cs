using Srinki.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Srinki;
using Srinki.services;

namespace Srinki
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!LoginPage.loginStatus())
                MainPage = new LoginPage();
            else
                MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            try
            {
                DataService.getDataService().readDataFromFile();
            }
            catch(Exception)
            {

            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
