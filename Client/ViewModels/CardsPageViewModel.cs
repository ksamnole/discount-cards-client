using System;
using System.Windows.Input;
using Client.Entities;
using Client.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class CardsPageViewModel : BaseViewModel
    {
        public ICommand GetAllUserCardsAsyncCommand { get; }
        public ObservableCollection<CardEntity> Cards { get; }
        public event Action OnRefreshCardsCompleted;
 
        private readonly ICardsPageModel _cardsModel;

        public CardsPageViewModel()
        {
            _cardsModel = new CardsPageModel();
            Cards = new ObservableCollection<CardEntity>();
            GetAllUserCardsAsyncCommand = new Command(GetAllUserCardsAsync);
        }

        public async void GetAllUserCardsAsync()
        {
            // После добавления регистрации должен использовать id конкретного пользователя
            var fakeUserId = 1;
            
            var allCards = await _cardsModel.GetAllUserCardsAsync(fakeUserId);

            Cards.Clear();

            foreach (var card in allCards)
                Cards.Add(card);
            
            OnRefreshCardsCompleted?.Invoke();
        }
    }
}