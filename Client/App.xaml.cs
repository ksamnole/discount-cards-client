using Client.Views;
using Client.Views.Auth;
using Xamarin.Forms;

namespace Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            if (Application.Current.Properties.ContainsKey("User"))
            {
                var login = Application.Current.Properties["User"] as string;
                MainPage = new NavigationPage(new CardsPage(login))
                {
                    BarBackgroundColor = Color.White
                };
            }
            else
            {
                MainPage = new RegistrationPage();
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
