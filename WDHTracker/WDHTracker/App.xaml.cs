using Plugin.SecureStorage;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace WDHTracker
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            // MainPage = new NavigationPage(new Login());
            //MainPage = new NavigationPage(new Login());
            String currentState = CrossSecureStorage.Current.GetValue("token");
            if (string.IsNullOrEmpty(currentState))
            {
                currentState = "0";
            }
            if (currentState.Equals("1"))
            {

               
                Application.Current.MainPage = new NavigationPage(new Front());
                
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new Login());
            }
        

        }

        protected override void OnStart ()
		{
            String currentUsername = CrossSecureStorage.Current.GetValue("username");
            String currentPassword = CrossSecureStorage.Current.GetValue("password");
            if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentPassword))
            {
                CrossSecureStorage.Current.DeleteKey("username");
                CrossSecureStorage.Current.DeleteKey("password");
                CrossSecureStorage.Current.DeleteKey("token");
                Application.Current.MainPage = new NavigationPage(new Login());

            }

        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            String currentUsername = CrossSecureStorage.Current.GetValue("username");
            String currentPassword = CrossSecureStorage.Current.GetValue("password");
            if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentPassword))
            {
                CrossSecureStorage.Current.DeleteKey("username");
                CrossSecureStorage.Current.DeleteKey("password");
                CrossSecureStorage.Current.DeleteKey("token");
                Application.Current.MainPage = new NavigationPage(new Login());

            }
        }
	}
}
