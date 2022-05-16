using System;
using System.Windows.Input;
using Client.Entities;
using Client.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Acr.UserDialogs;
using Client.Entities.Card;
using Client.Models.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class CardsPageViewModel : BaseViewModel
    {
        public ICommand GetAllUserCardsAsyncCommand { get; }
        public ObservableCollection<Card> Cards { get; }
        public event Action OnRefreshCardsCompleted;
 
        private readonly ICardsPageModel _cardsModel;
        private readonly string _login;

        public CardsPageViewModel(string login)
        {
            _login = login;
            _cardsModel = new CardsPageModel();
            Cards = new ObservableCollection<Card>();
            GetAllUserCardsAsyncCommand = new Command(GetAllUserCardsAsync);
        }

        public async void GetAllUserCardsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await UserDialogs.Instance.AlertAsync("Отсутствует подключение к интернету");
                return;
            }
            
            var allCards = await _cardsModel.GetAllUserCardsAsync(_login);

            Cards.Clear();

            foreach (var card in allCards)
                Cards.Add(card);
            
            OnRefreshCardsCompleted?.Invoke();
        }
    }
}