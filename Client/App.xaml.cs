using System;
using System.IO;
using Client.Data;
using Client.Views;
using Client.Views.Auth;
using Xamarin.Forms;

namespace Client
{
    public partial class App : Application
    {
        private static CardDB _cardDb;
        private static ShopDB _shopDb;

        public static CardDB CardDb =>
            _cardDb ?? (_cardDb = new CardDB(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cardDb.db3")));
        
        public static ShopDB ShopDb =>
            _shopDb ?? (_shopDb = new ShopDB(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "shopDb.db3")));

        public App()
        {
            InitializeComponent();

            if (Current.Properties.ContainsKey("User"))
            {
                var login = Current.Properties["User"] as string;
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
