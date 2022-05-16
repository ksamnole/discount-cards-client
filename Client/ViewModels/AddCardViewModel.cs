using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities;
using Client.Entities.Card;
using Client.Entities.Enums;
using Client.Models;
using Client.Models.Interfaces;
using Client.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;

namespace Client.ViewModels
{
    public class AddCardViewModel : BaseViewModel
    {
        public ICommand AddNewCardCommand { get; }
        public ICommand ScanCodeResultCommand { get; }
        public string CardNumber { get; set; }
        public int CurrentShopIndex { get; set; }
        public List<string> Shops => Enum.GetNames(typeof(Shops)).ToList();
        public event Action OnNewCardAdded;
        
        private readonly IAddCardModel _addCardModel;
        private readonly INavigation _navigation;
        private readonly string _login;

        private BarcodeFormat standart { get; set; }

        public AddCardViewModel(INavigation navigation, string login)
        {
            _login = login;
            _navigation = navigation;
            _addCardModel = new AddCardModel();
            AddNewCardCommand = new Command(AddNewCard);
            ScanCodeResultCommand = new Command<ZXing.Result>(ScanCodeResult);

            CardNumber = "0000000000000";
        }

        private void ScanCodeResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                CardNumber = result.Text;
                standart = result.BarcodeFormat;
                NotifyPropertyChanged(nameof(CardNumber));
            });
        }

        private async void AddNewCard()
        {
            if (CardNumber == "0000000000000")
            {
                await UserDialogs.Instance.AlertAsync("Номер карты не должен быть пустым");
                return;
            }
            
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await UserDialogs.Instance.AlertAsync("Отсутствует подключение к интернету");
                return;
            }
            
            await _addCardModel.AddNewCardAsync(new CreateCardEntity()
            {
                UserLogin = _login,
                ShopId = CurrentShopIndex - 1, // Other имеет id -1, а в enum id = 0, поэтому вычетаем
                Number = CardNumber,
                Standart = (int)standart
            });

            OnNewCardAdded?.Invoke();
            
            await _navigation.PopAsync();
        }
    }
}