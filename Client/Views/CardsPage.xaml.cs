using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client.Entities;
using Client.ViewModels;
using Client.Views.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardsPage : ContentPage
    {
        public CardsPage()
        {
            InitializeComponent();
            
            var cardsPageViewModel = new CardsPageViewModel();
            var addCardPageViewModel = new AddCardViewModel(Navigation);
            
            BindingContext = cardsPageViewModel;
            
            ProfileButton.Clicked += async (sender, args) => await Navigation.PushAsync(new ProfilePage());
            AddCardButton.Clicked += async (sender, args) => await Navigation.PushAsync(new AddCardPage(addCardPageViewModel));
            
            cardsPageViewModel.OnRefreshCardsCompleted += () => ListViewCards.IsRefreshing = false;
            addCardPageViewModel.OnNewCardAdded += () => cardsPageViewModel.GetAllCardsAsync();
            
            cardsPageViewModel.GetAllCardsAsync();
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
            var card = e.Item as CardEntity;
            await Navigation.PushAsync(new CardPage(card));
        }
    }
}
