using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities;
using Client.Models;
using Client.Models.Interfaces;
using Client.Views;
using Client.Views.Auth;
using Xamarin.Forms;

namespace Client.ViewModels.Auth
{
    public class RegistrationViewModel
    {
        public ICommand RegistrationCommand { get; }
        public ICommand GoToLoginPageCommand { get; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        private readonly IRegistrationModel _registrationModel;

        public RegistrationViewModel()
        {
            _registrationModel = new RegistrationModel();
            RegistrationCommand = new Command(Registration);
            GoToLoginPageCommand = new Command(() => Application.Current.MainPage = new LoginPage());
        }

        private async void Registration()
        {
            if (Password != ConfirmPassword)
            {
                UserDialogs.Instance.Alert("Пароли должны быть равны");
                return;
            }

            var res = await _registrationModel.Registration(new UserEntity()
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