using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities;
using Client.Models;
using Client.Models.Interfaces;
using Client.Views;
using Client.Views.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Client.ViewModels.Auth
{
    public class LoginViewModel
    {
        public ICommand AuthenticationCommand { get; }
        public ICommand GoToRegistrationPageCommand { get; }
        public string Login { get; set; }
        public string Password { get; set; }
        
        private readonly ILoginModel _loginModel;

        public LoginViewModel()
        {
            _loginModel = new LoginModel();
            AuthenticationCommand = new Command(Authentication);
            GoToRegistrationPageCommand = new Command( () => Application.Current.MainPage = new RegistrationPage());
        }

        private async void Authentication()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await UserDialogs.Instance.AlertAsync("Отсутствует подключение к интернету");
                return;
            }
            
            var res = await _loginModel.Authentication(new UserEntity()
                {
                    Login = Login,
                    Password = Password
                });
            
            if (res)
                Application.Current.MainPage = new NavigationPage(new CardsPage(Login))
                {
                    BarBackgroundColor = Color.White
                };
        }
    }
}