using System;
using Client.Entities;
using Client.Entities.Card;
using Client.ViewModels;
using Client.Views.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsPage : ContentPage
    {
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
            base.OnAppearing();
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
