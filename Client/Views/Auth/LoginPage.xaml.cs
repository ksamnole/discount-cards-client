using Client.ViewModels.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetIconColor(this, Color.Black);
            NavigationPage.SetHasNavigationBar(this, false);
            
            BindingContext = new LoginViewModel();
        }
    }
}