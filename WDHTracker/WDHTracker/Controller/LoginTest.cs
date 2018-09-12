using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using WDHTracker.Basic;
using WDHTracker.Models;

namespace WDHTracker.Controller
{
     class LoginTest : BasicRestClient
    {

        
        

        public LoginTest() : base(new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(CrossSecureStorage.Current.GetValue("username"), CrossSecureStorage.Current.GetValue("password"), "emea") }), null)
        {
            base._httpClient.BaseAddress = new Uri("http://miaodev.oticon.dk/");
            this.Get<List<InstrumentDetails>>($"api/v2.2/instrument/UserInstruments?userInitials=niko", false);
        }

        
    }
}
