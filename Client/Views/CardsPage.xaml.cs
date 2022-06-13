using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Entities.Card;
using Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsPage : ContentPage
    {
        public static bool NeedRefresh { get; set; }
        
        private bool isFirstLaunch { get; set; }
        
        private static CancellationTokenSource cts;
        
        private readonly CardsPageViewModel _cardsPageViewModel;
        private readonly AddCardViewModel _addCardPageViewModel;

        public CardsPage(string login)
        {
            InitializeComponent();

            _cardsPageViewModel = new CardsPageViewModel(Navigation, login);
            _addCardPageViewModel = new AddCardViewModel(Navigation, login);

            BindingContext = _cardsPageViewModel;
            
            ProfileButton.Clicked += async (sender, args) => await Navigation.PushAsync(new ProfilePage(login));
            AddCardButton.Clicked += async (sender, args) => await Navigation.PushAsync(new AddCardPage(_addCardPageViewModel));
            
            _cardsPageViewModel.OnRefreshCardsCompleted += () => ListViewCards.IsRefreshing = false;
            
            _cardsPageViewModel.GetNearestCard(true);
        }

        protected override void OnAppearing()
        {
            if (NeedRefresh)
            {
                _cardsPageViewModel.GetAllUserCardsAsync();

                NeedRefresh = false;
            }

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            
            base.OnDisappearing();
        }

        public static async Task<Location> GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                return location;
            }
            catch
            {
                return null;
            }
        }

        private async void Card_OnClick(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;

            if (sender is ListView lv) lv.SelectedItem = null;

            var card = e.Item as Card;

            await Navigation.PushAsync(new CardPage(card));
        }
    }
}
