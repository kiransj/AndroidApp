using Srinki.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Srinki
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new Srinki.BoothInformationPage();
		}

		protected override void OnStart ()
		{
            // Handle when your app starts
            //BoothInformation bi = new BoothInformation();
            //bi.updateBoothInformation();
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
