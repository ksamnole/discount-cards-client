using System;
using Client.ViewModels;
using Xamarin.Forms;

namespace Client.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();

            ProfileButton.Clicked += async (sender, args) => await Navigation.PushAsync(new ProfilePage());
            AddCardButton.Clicked += async (sender, args) => await Navigation.PushAsync(new AddCardPage());
        }
    }
}
