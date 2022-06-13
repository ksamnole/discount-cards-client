using System;
using System.Collections.Generic;
using System.Windows.Input;
using Client.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities.Card;
using Client.Models.Interfaces;
using Client.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class CardsPageViewModel : BaseViewModel
    {
        public ICommand GetAllUserCardsAsyncCommand { get; }
        public ICommand GetNearestCardCommand { get; }
        public ObservableCollection<Card> Cards { get; set; }
        public event Action OnRefreshCardsCompleted;

        private readonly INavigation _navigation;
        private readonly ICardsPageModel _cardsModel;
        private readonly IAddCardModel _addCardModel;
        private readonly ICardPageModel _cardModel;
        private readonly string _login;

        public CardsPageViewModel(INavigation navigation, string login)
        {
            _navigation = navigation;
            _login = login;
            _addCardModel = new AddCardModel();
            _cardsModel = new CardsPageModel();
            _cardModel = new CardPageModel();
            Cards = new ObservableCollection<Card>();
            GetAllUserCardsAsyncCommand = new Command(async () => await GetAllUserCardsAsync());
            GetNearestCardCommand = new Command(() => GetNearestCard());
        }

        public async void GetNearestCard(bool needGetCards = false)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await UserDialogs.Instance.AlertAsync("Отсутствует подключение к интернету");
                return;
            }
            
            if (needGetCards)
                await GetAllUserCardsAsync();
            
            if (Cards.Count == 0) return;

            var location = await CardsPage.GetCurrentLocation();

            if (location == null)
            {
                await UserDialogs.Instance.AlertAsync("Не удалось определить где вы находитесь");
                return;
            }
            
            await ShopLocationModel.UpdateDistanceOnCards(location.Longitude, location.Latitude, Cards);
                
            var cardsFromLocalDb = await App.CardDb.GetOrderedByDistanceCardsAsync();
            
            Cards.Clear();

            foreach (var card in cardsFromLocalDb)
                Cards.Add(card);
                
            await _navigation.PushAsync(new CardPage(Cards[0]));
        }

        public async Task GetAllUserCardsAsync()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await SyncCards();

                var cardsFromRemoteDb = await _cardsModel.GetAllUserCardsAsync(_login);
                
                if (cardsFromRemoteDb != null)
                    await App.CardDb.InsertCardsFromServer(cardsFromRemoteDb);
            }
            
            var cardsFromLocalDb = await App.CardDb.GetOrderedByDistanceCardsAsync();
            
            Cards.Clear();

            foreach (var card in cardsFromLocalDb)
                Cards.Add(card);

            OnRefreshCardsCompleted?.Invoke();
        }
        
        private async Task SyncCards()
        {
            if (!Application.Current.Properties.ContainsKey("NeedSync")) return;

            var deletedCards = await App.DeletedCardDb.GetAllAsync();

            foreach (var deletedCard in deletedCards)
            {
                await _cardModel.DeleteCardAsync(deletedCard.Number);

                await App.DeletedCardDb.DeleteCardByNumber(deletedCard.Number);
            }
            
            var notSyncCards = await App.CardDb.GetNotSyncCards();

            foreach (var card in notSyncCards)
            {
                var newCardId = await _addCardModel.AddNewCardAsync(new CreateCardEntity()
                {
                    UserLogin = _login,
                    ShopName = card.ShopName,
                    Number = card.Number,
                    Standart = (int)card.Standart
                });
                
                if (newCardId == -1) return;
                
                card.IsSync = true;
        
                await App.CardDb.UpdateCard(card);
            }

            Application.Current.Properties.Remove("NeedSync");
        }

    }
}