using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace WDHTracker
{
 
    public partial class Overview : ContentPage
    {
        public ZXing.Net.Mobile.Forms.ZXingScannerView scanner;
        public Overview()
        {
            InitializeComponent();
          
            
        }

        public void OnClickScan(object sender, EventArgs e)
        {
            ScanAsync();
        }

        public async void ScanAsync()
        {

            var scanPage = new ZXingScannerPage();

            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            // Navigate to our scanner page
            await Navigation.PushAsync(scanPage);

        }
    }
}