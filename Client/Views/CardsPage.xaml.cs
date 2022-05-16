using System;
using System.Threading;
using System.Threading.Tasks;
using Client.Entities.Card;
using Client.ViewModels;
using Client.Views.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsPage : ContentPage
    {
        private CancellationTokenSource cts;
        
        private readonly CardsPageViewModel _cardsPageViewModel;
        
        public CardsPage(string login)
        {
            InitializeComponent();

            _cardsPageViewModel = new CardsPageViewModel(login);
            var addCardPageViewModel = new AddCardViewModel(Navigation, login);

            BindingContext = _cardsPageViewModel;
            
            ProfileButton.Clicked += async (sender, args) => await Navigation.PushAsync(new ProfilePage(login));
            AddCardButton.Clicked += async (sender, args) => await Navigation.PushAsync(new AddCardPage(addCardPageViewModel));

            _cardsPageViewModel.OnRefreshCardsCompleted += () => ListViewCards.IsRefreshing = false;
            addCardPageViewModel.OnNewCardAdded += () => _cardsPageViewModel.GetAllUserCardsAsync();
        }

        protected override void OnAppearing()
        {
            _cardsPageViewModel.GetAllUserCardsAsync();
            
            GetCurrentLocation();
            
            base.OnAppearing();
        }
        
        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            
            base.OnDisappearing();
        }

        private async void GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private async void GoToRegistrationPage(object sender, EventArgs e)
        {
            // TEST LOGIN AND REGISTER
            await Navigation.PushAsync(new RegistrationPage());
        }
        
        private async void GoToLoginPage(object sender, EventArgs e)
        {
            // TEST LOGIN AND REGISTER
            await Navigation.PushAsync(new LoginPage());
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
