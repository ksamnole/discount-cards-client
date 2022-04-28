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
            
            var viewModel = new CardsPageViewModel();
            BindingContext = viewModel;
            
            ProfileButton.Clicked += async (sender, args) => await Navigation.PushAsync(new ProfilePage());
            AddCardButton.Clicked += async (sender, args) => await Navigation.PushAsync(new AddCardPage());
            
            viewModel.OnRefreshCardsCompleted += () => ListViewCards.IsRefreshing = false;
            viewModel.GetAllCardsAsync();
        }

        private async void Card_OnClick(object sender, EventArgs e)
        {
            var imageSource = ((ImageButton)sender).Source;
            await Navigation.PushAsync(new CardPage(imageSource));
        }
    }
}
