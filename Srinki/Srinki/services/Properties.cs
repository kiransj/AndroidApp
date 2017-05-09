using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Srinki.services
{
    class Properties
    {
        static public string lastUpdatedTime
        {
            get
            {
                string time = "";
                if (Application.Current.Properties["lastUpdatedTime"] != null)
                    time = Application.Current.Properties["lastUpdatedTime"].ToString();
                return time;
            }
        }

        static public void setLastUpdatedTime()
        {
            Application.Current.Properties["lastUpdatedTime"] = DateTime.Now.ToString("dd-MM-yy HH:mm");            
            Application.Current.SavePropertiesAsync();
        }
    }
}
