using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Srinki
{
    class IntentService
    {
        public static void SendData(string data)
        {
            Intent intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraText, data);
            intent.SetType("text/plain");
            Forms.Context.StartActivity(intent);
        }

        public static void Call(string phoneNumber)
        {
            Intent intent = new Intent(Intent.ActionDial, Android.Net.Uri.Parse("tel:" + phoneNumber));
            Forms.Context.StartActivity(intent);
        }
    }
}
