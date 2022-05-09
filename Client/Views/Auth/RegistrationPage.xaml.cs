using Client.ViewModels.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views.Auth
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            NavigationPage.SetIconColor(this, Color.Black);
            NavigationPage.SetHasNavigationBar(this, false);

            BindingContext = new RegistrationViewModel();
        }
    }
}