using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDHTracker.ViewModel;
using Xam.Plugin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WDHTracker
{

	public partial class Front : TabbedPage
    {
        Page overview;
        Page barcode;
        Page findInstrument;
        public PopupMenu Popup;

        public MenuViewModel ViewModel => MenuViewModel.Instance;
        public Front()

        {


            NavigationPage.SetHasBackButton(this, false);
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();



            this.Title = "Welcome " + CrossSecureStorage.Current.GetValue("username").ToUpper();

            BarBackgroundColor = Color.FromHex("#484559");

            BarTextColor = Color.White;

            Popup = new PopupMenu()
            {
                BindingContext = ViewModel
            };

           

            var doneItem = new ToolbarItem
            {
              
                Icon = "logout.png"
                
            };

            var infoItem = new ToolbarItem
            {

                Icon = "info.png"

            };
            this.ToolbarItems.Add(infoItem);
            this.ToolbarItems.Add(doneItem);

            doneItem.Clicked += async (object sender, System.EventArgs e) =>
            {
                var answer = await DisplayAlert("Logout", "Do you want to logout? ", "Yes", "No");
                if (answer)
                {
                    ApiClientWinAuthExampleSingleton.Clear();
                    CrossSecureStorage.Current.DeleteKey("username");
                    CrossSecureStorage.Current.DeleteKey("password");
                    CrossSecureStorage.Current.DeleteKey("token");
                    Application.Current.MainPage = new NavigationPage(new Login());
                    await Navigation.PopToRootAsync(true);
                }

             };

            infoItem.Clicked += async (object sender, System.EventArgs e) =>
            {

                await DisplayAlert("About", "Official MIAO App, created by NIKO", "Thanks");

            };


            barcode = new TestScan();
            overview = new MainPage();
            findInstrument = new FindInstrument();

            this.CurrentPageChanged += (object sender, EventArgs e) => {
                var i = this.Children.IndexOf(this.CurrentPage);
                if (i == 1)
                {
                    MessagingCenter.Send<Front>(this, "itemPage");
                }
            };



            Children.Add(barcode);
            Children.Add(overview);
            Children.Add(findInstrument);
            

            var pages = Children.GetEnumerator();
           // pages.MoveNext(); // First page
           // pages.MoveNext(); // Second page
            //CurrentPage = null;

        }
       

        

        public void SwitchToTab1()
        {
            CurrentPage = barcode;
        }
        


    }
}