using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Xml.Linq;
using ProgressRingControl.Forms.Plugin;
using Plugin.SecureStorage;

namespace WDHTracker
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestScan : ContentPage
	{
        ZXingScannerView scanner;
        ZXingDefaultOverlay overlay;
        Entry miaoCodeEntry;
        int UIDeviceSize;

        ActivityIndicator activitySpinner;
        Label mobileInfo;
        ListView testView;
        public TestScan () : base()
        {
           
            InitializeComponent ();
            
            //var secureStorage = DependencyService.Get<IPlugInProvider>().SecureStorage;

            activitySpinner = new ActivityIndicator();
           

            scanner = new ZXingScannerView();
            overlay = new ZXingDefaultOverlay();
            
            scanner.IsVisible = false;
            miaoCodeEntry = new Entry();
       
            miaoCodeEntry.VerticalOptions = LayoutOptions.Center;
            miaoCodeEntry.HorizontalOptions = LayoutOptions.FillAndExpand;
            miaoCodeEntry.HorizontalTextAlignment = TextAlignment.Center;
            miaoCodeEntry.IsEnabled = false;
            //miaoCodeEntry.MaxLength = 13;
            miaoCodeEntry.Placeholder = "Scan or type in MIAO ID";
            miaoCodeEntry.Keyboard = Keyboard.Telephone;
            miaoCodeEntry.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));

            mainGrid.Children.Add(miaoCodeEntry,0,0);


            mobileInfo = new Label
            {
                Text = "No devices",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry))


            };
            testView = new ListView();
            //mainGrid.Children.Add(testView, 0, 2);
            
            //mainGrid.Children.Add(mobileInfo, 0, 2);



            if (!string.IsNullOrEmpty(miaoCodeEntry.Text))
            {
                UIDeviceSize = miaoCodeEntry.Text.Length;
            }
            else
            {
                UIDeviceSize = 0;
            }

            miaoCodeEntry.TextChanged += (s, e) =>
            {
                if (miaoCodeEntry.Text.Length < UIDeviceSize)
                {
                    System.Diagnostics.Debug.WriteLine("Reel " + miaoCodeEntry.Text.Length + " Other " + UIDeviceSize);
                }
                else
                {
                    if (miaoCodeEntry.Text.Length == 3)
                    {

                        miaoCodeEntry.Text += "-";

                    }
                    if (miaoCodeEntry.Text.Length == 6)
                    {

                        miaoCodeEntry.Text += "-";

                    }
                    if (miaoCodeEntry.Text.Length == 10)
                    {

                        miaoCodeEntry.Text += "-";

                    }
                }


                UIDeviceSize = miaoCodeEntry.Text.Length;
            };



        }
        public void OnClickStart(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text.Equals("Scan"))
            {
                editButton.IsEnabled = false;
                button.BackgroundColor = Color.Blue;
                button.Text = "Scanning...";
                mainGrid.Children.Remove(miaoCodeEntry);
                mainGrid.Children.Add(scanner, 0, 0);
                scanner.IsVisible = true;
                Scan();

            }
            else if (button.Text.Equals("Scanning..."))
            {
                scanner.IsVisible = false;
                editButton.IsEnabled = true;

                button.BackgroundColor = Color.Accent;
                button.Text = "Scan";
                scanner.IsAnalyzing = false;
                mainGrid.Children.Remove(scanner);
                mainGrid.Children.Add(miaoCodeEntry, 0, 0);
            }
        }
      
        public void OnClickEdit(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text.Equals("Edit Code"))
            {
                scanButton.IsEnabled = false;
                button.Text = "Lock";
                button.BackgroundColor = Color.Blue;
                miaoCodeEntry.IsEnabled = true;
               
                miaoCodeEntry.Focus();
                
                
            }
            else if (button.Text.Equals("Lock"))
            {
                
                scanButton.IsEnabled = true;
                button.Text = "Edit Code";
                miaoCodeEntry.IsEnabled = false;
                button.BackgroundColor = Color.Accent;
                if (!string.IsNullOrEmpty(miaoCodeEntry.Text) && miaoCodeEntry.Text.Length == 13)
                {
                    ExampleMethodAsync(miaoCodeEntry.Text);
                }
                else
                {
                    DisplayAlert("Not valid", "This is not a valid code: " + miaoCodeEntry.Text, "OK");
                }
            }
        }

        public async void OnClickRent(object sender, EventArgs e)
        {
            var reponse = "";
            try
            {
                String codeToMove = miaoCodeEntry.Text.ToString();
                String formattedCode = codeToMove.Replace("-", "");
                string user = CrossSecureStorage.Current.GetValue("username");
                reponse =  ApiClientWinAuthExampleSingleton.Instance.MoveInstrument(CrossSecureStorage.Current.GetValue("username"), formattedCode);
            }
            catch (Exception ex)
            {
                var why = ex.InnerException.Message;
            }
            if(reponse.Equals("Instrument has been moved."))
            { 
            rentItemBtn.IsVisible = false;
            miaoCodeEntry.Text = "";
            rentItemBtn.IsEnabled = false;
            rentItemBtn.IsVisible = false;
            var progressRing = new ProgressRing { RingThickness = 20, Progress = 0f };
            progressRing.RingProgressColor = Color.AliceBlue;
            mainGrid.Children.Add(progressRing, 0, 3);
            double count = 100;
            double step = count / 100;
            await progressRing.ProgressTo(step, 800, Easing.Linear);
            count++;
            await Task.Yield();
            if (progressRing.Progress == 100)
            {
                progressRing.RingProgressColor = Color.Green;
                
            }
            listviewSingleDevice.ItemsSource = null;
            await progressRing.FadeTo(0, 3000);
            MessagingCenter.Send<TestScan>(this, "itemRented");
            }
            else if (reponse.Contains("is assigned to another user"))
            {
                await DisplayAlert("Can't", reponse, "OK");
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong, please try again " + reponse, "OK");
            }
            
            

            //Have ASYNC METHODE CALL HERE TO UPLOAD TO MIAO!!!

        }


            public void Scan()
        {
            try
            {
                scanner.Options = new MobileBarcodeScanningOptions()
                {
                    UseFrontCameraIfAvailable = false, //update later to come from settings
                                                       //
                    //PossibleFormats = new List<BarcodeFormat>(),
                    TryHarder = true,
                    AutoRotate = false,
                    
                    //TryInverted = true,
                    DelayBetweenContinuousScans = 1000
                    
                };


                scanner.IsEnabled = true;
                scanner.IsScanning = true;
                scanner.IsAnalyzing = true;
                //scanner.IsVisible = false;
                //scanner.Options.PossibleFormats.Add(BarcodeFormat.QR_CODE);
                //scanner.Options.PossibleFormats.Add(BarcodeFormat.DATA_MATRIX);
                //scanner.Options.PossibleFormats.Add(BarcodeFormat.EAN_13);


                scanner.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (result.Text.Length == 10) {
                        editButton.IsEnabled = true;
                        scanButton.Text = "Scan";
                    scanButton.BackgroundColor = Color.Accent;
                    mainGrid.Children.Remove(scanner);
                    mainGrid.Children.Add(miaoCodeEntry, 0, 0);
                    miaoCodeEntry.Text = correctMiao(result.Text);
                    ExampleMethodAsync(miaoCodeEntry.Text);
                    }
                    else {

                        editButton.IsEnabled = true;
                        scanButton.Text = "Scan";
                        scanButton.BackgroundColor = Color.Accent;
                        mainGrid.Children.Remove(scanner);
                        mainGrid.Children.Add(miaoCodeEntry, 0, 0);
                        DisplayAlert("Not valid", "This is not a valid code: "+ result.Text, "OK");
                        

                    }
                });

               
                

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }


        private async void ExampleMethodAsync(String miaoCodeToCheck)
        {
            
            //await Task.Delay(2000); //PLACE HOLDER FOR NETOWRK COMMUNICATION!!
     
            mainGrid.Children.Add(activitySpinner, 0, 2);
            activitySpinner.IsRunning = true;
            if (NetworkCheck.IsInternet())
            {

                try
                {
                 //   Uri geturi = new Uri("http://miaodev.oticon.dk/Sisyphus/GetSearchInstruments?itemNumber=" + miaoCodeToCheck);
               

                List<MobileDetails> geturo = ApiClientWinAuthExampleSingleton.Instance.GetInstrument(miaoCodeEntry.Text);

                    if(geturo.Count > 0) { 

                    activitySpinner.IsRunning = false;
                    mainGrid.Children.Remove(activitySpinner);
                    listviewSingleDevice.ItemsSource = geturo;

                    rentItemBtn.IsEnabled = true;
                    rentItemBtn.IsVisible = true;
                    }
                    else
                    {
                        await DisplayAlert("Not found", "No device was found for the following ID " + miaoCodeEntry.Text, "OK");
                        activitySpinner.IsRunning = false;
                        mainGrid.Children.Remove(activitySpinner);
                    }
                }
                catch
                {
                    await DisplayAlert("Error", "Some weird error happent, try again", "OK");
                }

                //HttpClient client = new HttpClient();
                //HttpResponseMessage responseGet = await client.GetAsync(geturi);
                //string response = await responseGet.Content.ReadAsStringAsync();//Getting response  


                ////string contactsJson = await jsonText.Content.ReadAsStringAsync(); //Getting response  
                //activitySpinner.IsRunning = false;
                //mainGrid.Children.Remove(activitySpinner);
                //mobileInfo.Text = response;
                //try { 
                //XDocument doc = XDocument.Parse(response); //TRY CATCH EXECEPTION HERE!

                //XNamespace ns = "http://schemas.datacontract.org/2004/07/MIAO.Services.ViewModels.SisyphusAPI";

                //List<MobileDetails> MobileList = new List<MobileDetails>();
                //int inn = 0;
                //foreach (var item in doc.Descendants(ns + "SearchInstrumentViewModel"))
                //{
                //    //mobileInfo.Text = item.ToString();

                //    //"SearchInstrumentViewModel"
                //    MobileDetails objMobileItem = new MobileDetails();
                //    objMobileItem.ItemNumber = item.Element(ns + "ItemNumber").Value.ToString();
                //    objMobileItem.Model = item.Element(ns + "Model").Value.ToString();
                //    objMobileItem.Manufacturer = item.Element(ns + "Manufacturer").Value.ToString();

                //    MobileList.Add(objMobileItem);
                //}



            }

        }

        private async Task LstItems_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //listviewDevices.SelectedItem.ToString
            ListView lv = (ListView)sender;
            MobileDetails instrumendetails = (MobileDetails)lv.SelectedItem;


            await DisplayAlert(instrumendetails.Model, "Current owner: " + instrumendetails.LocationName + "\nItemNumber: "+instrumendetails.ItemNumber, "OK");
         
            lv.SelectedItem = null;

        }


        public String correctMiao(String result)
        {
            String miaoCorrect = null;

            var first = result.Substring(0, 3);
            var second = result.Substring(3, 2);
            var third = result.Substring(5, 3);
            var fourth = result.Substring(8);

            miaoCorrect = first + "-" + second + "-" + third + "-" + fourth;

            return miaoCorrect;
        }


        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    scanner.IsScanning = true;
        //}

        //protected override void OnDisappearing()
        //{
        //    scanner.IsScanning = false;

        //    base.OnDisappearing();
        //}

    }
}