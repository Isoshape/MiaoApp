using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System.Security.Cryptography;
using PCLCrypto;
using Plugin.SecureStorage;
using WDHTracker.Models;
using WDHTracker.Controller;

namespace WDHTracker
{

    public partial class Login : ContentPage
	{
        ActivityIndicator activitySpinner;
        public Login()
        {
            

            InitializeComponent();
            activitySpinner = new ActivityIndicator();
            activitySpinner.IsRunning = true;
            activitySpinner.IsVisible = false;
            mainLayout.Children.Add(activitySpinner);
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

            var list = new List<string> { "NIKO", "PEKL", "CLMA" , "CITO", "KITO", "KAKA" };
            NameView.ItemsSource = list;
            NameView.IsVisible = false;

            MessagingCenter.Subscribe<Basic.CoreRestClient>(this, "wrongPassword", async (sender) => {
                // do something whenever the "itemRented" message is sent
                CrossSecureStorage.Current.DeleteKey("username");
                CrossSecureStorage.Current.DeleteKey("password");
                CrossSecureStorage.Current.DeleteKey("token");
                await DisplayAlert("Unsuccessfull", "Wrong username or password", "Try again");
                activitySpinner.IsVisible = false;
            });


            //NameEntry.MaxLength = 4;

        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
     
            NameEntry.Text = e.SelectedItem.ToString();
            NameView.IsVisible = false;
           
        }


        private async Task AcceptClick(object sender, EventArgs e)
        {

            if (NetworkCheck.IsInternet())
            {

                if (string.IsNullOrEmpty(NameEntry.Text) || string.IsNullOrEmpty(PwEntry.Text))
                {
                    await DisplayAlert("Missing information", "Please type in both username and password", "OK");
                }
                else
                {
                    activitySpinner.IsVisible = true;

                    //Save to secure storage

                    CrossSecureStorage.Current.SetValue("username", NameEntry.Text);
                    CrossSecureStorage.Current.SetValue("password", PwEntry.Text);

                    var instrumetns = ApiClientO2AuthExampleSingleton.Instance.GetUserInstruments(CrossSecureStorage.Current.GetValue("username"));
                    //List<InstrumentDetails> instrumetns = null;

                    LoginTest tester = new LoginTest();

                    NetworkCheck.getLogged();
                    if (NetworkCheck.getLogged() == true)
                    {
                        CrossSecureStorage.Current.SetValue("username", NameEntry.Text);
                        CrossSecureStorage.Current.SetValue("password", PwEntry.Text);
                        CrossSecureStorage.Current.SetValue("token", "1");
                        activitySpinner.IsVisible = false;
                        Application.Current.MainPage = new NavigationPage(new Front());

                        await Navigation.PopToRootAsync(true);
                    }
                }
            }
            else
            {
                await DisplayAlert("Internet", "No internet available", "OK");
            }




        }





        //public void cryptPassWord()
        //{
        //    byte[] keyMaterial;
        //    byte[] data;
        //    var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.SymmetricAlgorithm.AesCbcPkcs7);
        //    var key = provider.CreateSymmetricKey(keyMaterial);

        //    // The IV may be null, but supplying a random IV increases security.
        //    // The IV is not a secret like the key is.
        //    // You can transmit the IV (w/o encryption) alongside the ciphertext.
        //    var iv = WinRTCrypto.CryptographicBuffer.GenerateRandom(provider.BlockLength);

        //    byte[] cipherText = WinRTCrypto.CryptographicEngine.Encrypt(key, data, iv);

        //    // When decrypting, use the same IV that was passed to encrypt.
        //    byte[] plainText = WinRTCrypto.CryptographicEngine.Decrypt(key, cipherText, iv);
        //}


    }
}