using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities.Card;
using Client.Entities.Shop;
using Client.Models;
using Client.Models.Interfaces;
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
            Shops = new List<string>();

            UpdateShops();
            
            CardNumber = "0000000000000";
        }

        public async void UpdateShops()
        {
            IEnumerable<Shop> shops;
            
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                shops = await _addCardModel.GetShopsAsync();

                await App.ShopDb.InsertShopsFromServer(shops);
            }
            else
            {
                shops = await App.ShopDb.GetShopsAsync();
            }
            
            Shops = shops.Select(x => x.Name).ToList();
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

            var shopName = Shops[CurrentShopIndex];

            if (shopName != "Другое")
            {
                if (!await App.CardDb.IsShopUnique(shopName))
                {
                    await UserDialogs.Instance.AlertAsync("Карта этого магазина уже добавлена");
                    return;
                }
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
                
                if (newCardId == -1) return;

                await App.CardDb.AddNewCard(new Card()
                {
                    Id = newCardId,
                    Number = CardNumber,
                    Standart = standart,
                    ShopName = shopName,
                    ImageSource = $"{Shops[CurrentShopIndex].Unidecode()}.png",
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
                    ImageSource = $"{Shops[CurrentShopIndex].Unidecode()}.png",
                    IsSync = false
                });
            }

            OnNewCardAdded?.Invoke();
            
            await _navigation.PopAsync();
        }
    }
}