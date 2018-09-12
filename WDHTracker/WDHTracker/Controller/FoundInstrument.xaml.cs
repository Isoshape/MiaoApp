using Plugin.SecureStorage;
using System;
using WDHTracker.Models;
using Xamarin.Forms;

using Plugin.Messaging;

namespace WDHTracker
{
    public partial class FoundInstrument : ContentPage
    {
        String itemNumber;

        internal FoundInstrument(InstrumentDetailsAll instrumendetails)
        {
            InstrumentDetailsAll test = instrumendetails;
            InitializeComponent();

            this.Title = ""+test.Model;
            this.itemNumber = test.ItemNumber;

            Number.Text = "" + test.ItemNumber;
            Name.Text = "" + test.Model;
            Manufactur.Text = "" + test.Manufacturer;

        }

        public async void OnClickRent(object sender, EventArgs e)
        {
            var reponse = "";
            try
            {
                String codeToMove = itemNumber;
                String formattedCode = codeToMove.Replace("-", "");
                string user = CrossSecureStorage.Current.GetValue("username");
                reponse = ApiClientWinAuthExampleSingleton.Instance.MoveInstrument(CrossSecureStorage.Current.GetValue("username"), formattedCode);
            }
            catch (Exception ex)
            {
                var why = ex.InnerException.Message;
            }
            if (reponse.Equals("Instrument has been moved."))
            {
                MessagingCenter.Send<FoundInstrument>(this, "itemRented");
                await DisplayAlert("Success", "Item is now rented", "Thanks");
                Navigation.RemovePage(this);
            }
            else if (reponse.Contains("is assigned to another user"))
            {
                await DisplayAlert("Can't", reponse, "OK");
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong, please try again " + reponse, "OK");
            }

            //var emailMessenger = CrossMessaging.Current.EmailMessenger;
            //if (emailMessenger.CanSendEmail)
            //{
            //    await DisplayAlert("Mail", "Sent", "ok");
            //    emailMessenger.SendEmail("niko@oticon.com", "Xamarin Messaging Plugin", "Well hello there from Xam.Messaging.Plugin");
            //}


        }


    }
}
