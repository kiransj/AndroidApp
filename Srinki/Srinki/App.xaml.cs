using Srinki.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Srinki
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Srinki.BoothInformationPage();
        }

        protected override void OnStart()
        {
            try
            {
                List<Information> components = new List<Information>();
                components.Add(new BoothInformation());
                components.Add(new RoadInformation());

                Parallel.ForEach(components, x => x.UpdateInformation());
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
