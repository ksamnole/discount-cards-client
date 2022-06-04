using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities;
using Client.Models;
using Client.Models.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public ICommand ChangePasswordCommand { get; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set;}
        public string NewPasswordConfirm { get; set;}

        private readonly string _login;
        private readonly IProfileModel _profileModel;

        public ProfileViewModel(string login)
        {
            _login = login;
            _profileModel = new ProfileModel();
            ChangePasswordCommand = new Command(ChangePassword);
        }

        private async void ChangePassword()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await UserDialogs.Instance.AlertAsync("Отсутствует подключение к интернету");
                return;
            }

            if (NewPassword != NewPasswordConfirm)
            {
                await UserDialogs.Instance.AlertAsync("Пароли должны быть одинаковые");
                return;
            }

            await _profileModel.ChangePassword(_login, CurrentPassword, NewPassword);
        }
    }
}