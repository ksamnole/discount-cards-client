using System;
using Client.Entities.Card;
using Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPage : ContentPage
    {
        public CardPage(Card card)
        {
            InitializeComponent();
            NavigationPage.SetIconColor(this, Color.Black);
            
            var cardPageViewModel = new CardPageViewModel(Navigation, card);
            BindingContext = cardPageViewModel;
            
            CardImage.Source = card.ImageSource;
            Title.Text = card.ShopName.ToString();
            CardNumber.Text = card.Number;
            
            BarcodeImage.Source = ImageSource.FromUri(new Uri($"http://bwipjs-api.metafloor.com/?bcid=ean13&text={card.Number}&scale=3"));
        }
    }
}