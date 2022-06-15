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

        private static CancellationTokenSource cts;
        private static Label searching;
        
        private readonly CardsPageViewModel _cardsPageViewModel;
        private readonly AddCardViewModel _addCardPageViewModel;

        public CardsPage(string login)
        {
            InitializeComponent();

            searching = Searching;

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
            searching.IsVisible = true;

            Task.Run(SearchingOpacityBlinking);
            
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
                searching.IsVisible = false;
                return location;
            }
            catch
            {
                searching.IsVisible = false;
                return null;
            }
            
        }

        private static async void SearchingOpacityBlinking()
        {
            for (var i = 1f; i >= 0; i -= 0.01f)
            {
                searching.Opacity = i;
                await Task.Delay(25);
            }
        
            for (var i = 0f; i <= 1; i += 0.01f)
            {
                searching.Opacity = i;
                await Task.Delay(25);
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
