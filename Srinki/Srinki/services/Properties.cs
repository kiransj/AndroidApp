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
                try
                {
                    return Application.Current.Properties["lastUpdatedTime"].ToString();
                }
                catch(Exception)
                {
                    return "";
                }
            }
        }

        static public void setLastUpdatedTime()
        {
            Application.Current.Properties["lastUpdatedTime"] = DateTime.Now.ToString("dd-MM-yy HH:mm");            
            Application.Current.SavePropertiesAsync();
        }
    }
}
