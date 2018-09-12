using WDHTracker.Basic;
using WDHTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.SecureStorage;

namespace WDHTracker
{
    class ApiClientWinAuthExampleSingleton : BasicRestClient
    {
        private static object _instanceLock = new object();
        #region Singleton
        private static  ApiClientWinAuthExampleSingleton _instance = new ApiClientWinAuthExampleSingleton();

       

        static ApiClientWinAuthExampleSingleton() { }

        private ApiClientWinAuthExampleSingleton() : base(new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(CrossSecureStorage.Current.GetValue("username"), CrossSecureStorage.Current.GetValue("password"), "emea") }), null)
        {
            base._httpClient.BaseAddress = new Uri("http://miaodev.oticon.dk");
            //http://miao.dgs.com/
            //"http://miaodev.oticon.dk/"
        }

        public static ApiClientWinAuthExampleSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ApiClientWinAuthExampleSingleton();
                        }
                    }
                }

                return _instance;
               
            }
            
        }

        public static void Clear()
        {
            if (_instance != null)
            {
                lock (_instanceLock)
                {
                    if (_instance != null)
                    {
                        _instance = null;
                    }
                }
            }
        }
        #endregion


        public List<InstrumentDetails> GetUserInstruments(string userInitials)
        {
            return this.Get<List<InstrumentDetails>>($"api/v2.2/instrument/UserInstruments?userInitials={userInitials}", false);
           
        }

        public string MoveInstrument(string locationInitials, string instrumentItemNo)
        {
            return this.Get<string>($"api/v2.2/instrument/move?locationInitials={locationInitials}&instrumentNumber={instrumentItemNo}", false);
        }

        public List<MobileDetails> GetInstrument(string instrumentItemNo)
        {
            return this.Get<List<MobileDetails>>($"api/v2.2/Sisyphus/GetSearchInstruments?itemNumber={instrumentItemNo}", false);
        }

        public List<LocationDetails> GetLocations()
        {
            return this.Get<List<LocationDetails>>($"api/v2.2/Sisyphus/GetLocations", false);
        }

        public List<MobileDetails> findInstruments(string modelName)
        {
            return this.Get<List<MobileDetails>>($"api/v2.0/Sisyphus/GetSearchInstruments?model={modelName}", false);
        }

        public List<MobileDetails> findAllInstruments()
        {
            return this.Get<List<MobileDetails>>($"api/v2.1/Sisyphus/GetSearchInstruments?", false);
        }

        //searchAllFieldsString
        public List<InstrumentDetailsAll> findInAllFields(string value)
        {
            return this.Get<List<InstrumentDetailsAll>>($"api/v2.2/instrument/SearchFullText/{value}", false);
        }

    }
}
