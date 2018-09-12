using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Settings;
using System.ComponentModel;
using System.Net.Http;
using System.Xml.Linq;
using Plugin.SecureStorage;
using WDHTracker.Models;


namespace WDHTracker
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
       

        public MainPage()
        {
            InitializeComponent();
           
            getData();

            listviewDevices.RefreshCommand = new Command(() => {
                //Do your stuff.    
                getData();
                listviewDevices.IsRefreshing = false;
            });


            MessagingCenter.Subscribe<TestScan>(this, "itemRented", (sender) => {
                // do something whenever the "itemRented" message is sent
                getData();
            });

            MessagingCenter.Subscribe<FoundInstrument>(this, "itemRented", (sender) => {
                // do something whenever the "itemRented" message is sent
                getData();
            });

            MessagingCenter.Subscribe<Details>(this, "itemReturned", (sender) => {
                // do something whenever the "itemReturned" message is sent
                getData();
            });

            MessagingCenter.Subscribe<Front>(this, "itemPage", (sender) => {
                // do something whenever the "itemPage" message is sent
                getData();
            });


        }

       


        private async void LstItems_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //listviewDevices.SelectedItem.ToString
            ListView lv = (ListView)sender;
        
            InstrumentDetails instrumendetails = (InstrumentDetails) lv.SelectedItem;
       
            var test = instrumendetails.ItemNumber;
            var newpage = new Details(instrumendetails);
            //CONSTRUCTUR REQUIREMENT HERE
            await Navigation.PushAsync(newpage);

        }


        public void getData()
        {
            if (NetworkCheck.IsInternet())
            {

                var instrumetns = ApiClientWinAuthExampleSingleton.Instance.GetUserInstruments(CrossSecureStorage.Current.GetValue("username"));

                if (instrumetns.Count > 0)
                {
                    listviewDevices.ItemsSource = instrumetns;
                   
                    itemLabel.Text = "My Items [" + instrumetns.Count() + "]";
               
                }
                else
                {
                    listviewDevices.ItemsSource = null;
                    itemLabel.Text = "You have no devices";
                }
               



            }
        }



    }
}
