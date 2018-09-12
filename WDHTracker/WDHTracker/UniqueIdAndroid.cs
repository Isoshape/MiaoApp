using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static Android.Provider.Settings;



[assembly: Xamarin.Forms.Dependency(typeof(WDHTracker.Droid.UniqueIdAndroid))]
namespace WDHTracker.Droid
{
    class UniqueIdAndroid : IDevice
    {
        String udCode;
        String modelNow = null;
        public string[] GetIdentifier()
        {
            
            var myList = new List<string>();
            ContentResolver context = Android.App.Application.Context.ContentResolver;
            udCode = Secure.GetString(context, Secure.AndroidId);
            modelNow = Android.OS.Build.Model;
            myList.Add(modelNow);
            myList.Add(udCode);
            var myArray = myList.ToArray();
            return myArray;
           




        }
    }
}
