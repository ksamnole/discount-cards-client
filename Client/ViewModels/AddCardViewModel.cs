using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities.Card;
using Client.Entities.Shop;
using Client.Models;
using Client.Models.Interfaces;
using Client.Views;
using Unidecode.NET;
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
        public List<string> Shops { get; set; }

        private readonly IAddCardModel _addCardModel;
        private readonly INavigation _navigation;
        private readonly string _login;

        private static bool _isInProccess { get; set; }

        private BarcodeFormat standart { get; set; }

        public AddCardViewModel(INavigation navigation, string login)
        {
            _login = login;
            _navigation = navigation;
            _addCardModel = new AddCardModel();
            AddNewCardCommand = new Command(AddNewCard);
            ScanCodeResultCommand = new Command<ZXing.Result>(ScanCodeResult);
            Shops = new List<string>();
            standart = BarcodeFormat.EAN_13;
            _isInProccess = false;

            UpdateShops();
            
            CardNumber = "0000000000000";
        }

        public async void UpdateShops()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var shopsFromRemoteDb = await _addCardModel.GetShopsAsync();

                await App.ShopDb.InsertShopsFromServer(shopsFromRemoteDb);
            }

            var shopsFromLocalDb = await App.ShopDb.GetShopsAsync();

            Shops = shopsFromLocalDb.Select(x => x.Name).ToList();
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
            if (_isInProccess) return;
            
            _isInProccess = true;
            
            if (CardNumber == "0000000000000")
            {
                await UserDialogs.Instance.AlertAsync("Номер карты не должен быть пустым");
                _isInProccess = false;
                return;
            }

            var shopName = Shops[CurrentShopIndex];
            var imageSource = $"{Shops[CurrentShopIndex].Unidecode().Replace(" ", "").Replace("&", "_")}.png";
            
            if (!await App.CardDb.IsShopUnique(shopName))
            {
                await UserDialogs.Instance.AlertAsync("Карта этого магазина уже добавлена");
                _isInProccess = false;
                return;
            }
            
            if (!await App.CardDb.IsCardNumberUnique(CardNumber))
            {
                await UserDialogs.Instance.AlertAsync("Номер карты должен быть уникален");
                _isInProccess = false;
                return;
            }

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var newCardId = await _addCardModel.AddNewCardAsync(new CreateCardEntity()
                {
                    UserLogin = _login,
                    ShopName = shopName,
                    Number = CardNumber,
                    Standart = (int)standart
                });

                if (newCardId == -1)
                {
                    _isInProccess = false;
                    return;
                }

                await App.CardDb.AddNewCard(new Card()
                {
                    Id = newCardId,
                    Number = CardNumber,
                    Standart = standart,
                    ShopName = shopName,
                    ImageSource = imageSource,
                    IsSync = true
                });

            }
            else
            {
                if (!Application.Current.Properties.ContainsKey("NeedSync"))
                    Application.Current.Properties.Add("NeedSync", 1);

                await App.CardDb.AddNewCard(new Card()
                {
                    Number = CardNumber,
                    Standart = standart,
                    ShopName = shopName,
                    ImageSource = imageSource,
                    IsSync = false
                });
            }

            CardsPage.NeedRefresh = true;

            _isInProccess = false;
            
            await _navigation.PopAsync();
        }
    }
}