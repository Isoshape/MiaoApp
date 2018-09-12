using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WDHTracker
{
    public class NetworkCheck
    {
        static Boolean logged = false;
        public static bool IsInternet()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                // write your code if there is no Internet available      
                return false;
            }
        }

    public static void setLogged(Boolean flag)
        {
            logged = flag;
        }

    public static bool getLogged()
        {
            return logged;
        }

    }
}
