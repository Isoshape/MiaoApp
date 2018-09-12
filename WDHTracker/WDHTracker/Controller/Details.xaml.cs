using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WDHTracker.Models;
using WDHTracker.ViewModel;
using Xamarin.Forms;

namespace WDHTracker
{
    public partial class Details : ContentPage, INotifyPropertyChanged
    {



        List<LocationDetails> locations;

        internal Details(InstrumentDetails instrumendetails)
        {
            InstrumentDetails test = instrumendetails;
            InitializeComponent();

            this.Title = ""+test.Model;

            Number.Text = "" + test.ItemNumber;
            Name.Text = "" + test.Model;
            Manufactur.Text = "" + test.Manufacturer;

            String ss = CrossSecureStorage.Current.GetValue("password");

            getData();
        }

        public void getData()
        {

            if (NetworkCheck.IsInternet())
            {
     

                locations = ApiClientWinAuthExampleSingleton.Instance.GetLocations();
                listviewLocations.ItemsSource = locations;
               

            }
        }

        public void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            listviewLocations.BeginRefresh();
            
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                listviewLocations.ItemsSource = locations;
            else
                //c => c.A
                listviewLocations.ItemsSource = locations.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToLower()) || i.LocationID.ToLower().Contains(e.NewTextValue.ToLower()));

            listviewLocations.EndRefresh();
        }

        private void LstLocation_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //listviewDevices.SelectedItem.ToString
            ListView lv = (ListView)sender;
            LocationDetails locationDetail = (LocationDetails)lv.SelectedItem;
            ShowExitDialog(locationDetail.Name, locationDetail.Initials);



        }

        private async void ShowExitDialog(String locationName, string locatioInitials)
        {
            var answer = await DisplayAlert("Do you want to move", "Move item to location "+locationName, "Yes", "No");
            if (answer)
            {
                moveItemAsync(locatioInitials);
            }
        }

        public async void moveItemAsync(String locatioInitials)
        {
            var reponse = "";
            try
            {
                String codeToMove = Number.Text.ToString();
                String formattedCode = codeToMove.Replace("-", "");
                //CrossSecureStorage.Current.GetValue("username")
                reponse = ApiClientWinAuthExampleSingleton.Instance.MoveInstrument(locatioInitials, formattedCode);
            }
            catch (Exception ex)
            {
                var why = ex.InnerException.Message;
            }
            if (reponse.Equals("Instrument has been moved."))
            {
                MessagingCenter.Send<Details>(this, "itemReturned");
                Navigation.RemovePage(this);
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong, please try again", "OK");
            }
        }

    }
}
