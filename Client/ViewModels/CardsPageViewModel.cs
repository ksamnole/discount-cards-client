using System;
using System.Collections.Generic;
using System.Windows.Input;
using Client.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        public event Action OnGetNewLocation;
        
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
            GetAllUserCardsAsyncCommand = new Command(GetAllUserCardsAsync);
            GetNearestCardCommand = new Command(GetNearestCard);
        }

        public async void GetNearestCard()
        {
            var location = CardsPage.Location;
            
            if (location != null)
            {
                await ShopLocationModel.UpdateDistanceOnCards((float)location.Longitude, (float)location.Latitude, Cards);

                await _navigation.PushAsync(new CardPage(Cards[0]));
            }
        }

        public async void GetAllUserCardsAsync()
        {
            OnGetNewLocation?.Invoke();
            
            IEnumerable<Card> allCards;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await SyncCards();
                
                allCards = await _cardsModel.GetAllUserCardsAsync(_login);
                
                if (allCards != null)
                    await App.CardDb.InsertCardsFromServer(allCards);
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

                card.Id = newCardId;
                card.IsSync = true;
        
                await App.CardDb.UpdateCard(card);
            }

            if (CardPageViewModel.NotActiveCardNumbers != null)
            {
                foreach (var number in CardPageViewModel.NotActiveCardNumbers)
                {
                    await _cardModel.DeleteCardAsync(number);
                }
            }

            Application.Current.Properties.Remove("NeedSync");
        }

    }
}