using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDHTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;

namespace WDHTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FindInstrument : ContentPage
	{

        List<InstrumentDetailsAll> devices;
    
        public FindInstrument()
        {
            InitializeComponent();
            

        }

        public async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
           string device = searchDeviceBar.Text;

            if (e.NewTextValue == null)
            {
                listviewdevices.ItemsSource = null;
            }

            //if (device.Count() > 1)
            //{
            await Task.Delay(1000);
            getData(device);
                listviewdevices.ItemsSource = null;
                listviewdevices.ItemsSource = devices;
            //}
            //    listviewdevices.BeginRefresh();

            //    if (string.IsNullOrWhiteSpace(e.NewTextValue))
            //        listviewdevices.ItemsSource = devices;
            //    else
            //        //c => c.A
                    
            //       listviewdevices.ItemsSource = devices.Where(i => i.Model.ToLower().Contains(e.NewTextValue.ToLower()) );
            //    //i.Model.ToLower().Contains(e.NewTextValue.ToLower()) || 
            //    //|| i.Manufacturer.ToLower().Contains(e.NewTextValue.ToLower())

            //    listviewdevices.EndRefresh();
            //}
            //else
            //{
            //    listviewdevices.ItemsSource = "no devices";
            //}
        }


        private async Task LstItems_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //listviewDevices.SelectedItem.ToString
            ListView lv = (ListView)sender;
            InstrumentDetailsAll instrumendetails = (InstrumentDetailsAll)lv.SelectedItem;

          
            var newpage = new FoundInstrument(instrumendetails);
            ////CONSTRUCTUR REQUIREMENT HERE
            await Navigation.PushAsync(newpage);

        }


        public void getData(string device)
        {

            if (NetworkCheck.IsInternet())
            {

                devices = ApiClientWinAuthExampleSingleton.Instance.findInAllFields(device);
                //listviewdevices.ItemsSource = devices;


            }
        }

    }
}