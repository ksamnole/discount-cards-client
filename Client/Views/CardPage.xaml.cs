using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPage : ContentPage
    {
        public CardPage(CardEntity card)
        {
            InitializeComponent();
            NavigationPage.SetIconColor(this, Color.Black);
            
            Card.Source = card.ImageSource;
            Title.Text = card.Name;
            CardNumber.Text = card.Number;
        }
    }
}