using Client.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(string login)
        {
            InitializeComponent();
            NavigationPage.SetIconColor(this, Color.Black);

            BindingContext = new ProfileViewModel(login);

            Login.Text = login;
        }
    }
}