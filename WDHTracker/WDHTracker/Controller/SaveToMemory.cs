using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace WDHTracker
{
    class SaveToMemory
    {
        private static Plugin.Settings.Abstractions.ISettings AppSettings =>
         CrossSettings.Current;


        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }

    }


}
