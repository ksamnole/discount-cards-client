using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCardPage : ContentPage
    {
        public AddCardPage(AddCardViewModel addCardViewModel)
        {
            InitializeComponent();
            BindingContext = addCardViewModel;
            NavigationPage.SetIconColor(this, Color.Black);
        }
    }
}