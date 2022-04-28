using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client.ViewModels;
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

        private async void Card_OnClick(object sender, EventArgs e)
        {
            var imageSource = ((ImageButton)sender).Source;
            await Navigation.PushAsync(new CardPage(imageSource));
        }
    }
}
