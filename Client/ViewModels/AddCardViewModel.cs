using System;
using System.Windows.Input;
using Client.Models;
using Client.Views;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class AddCardViewModel : BaseViewModel
    {
        public ICommand AddNewCardCommand { get; }
        public string CardNumber { get; set; }
        public event Action OnNewCardAdded;
        
        private readonly IAddCardModel _addCardModel;
        private readonly INavigation _navigation;

        public AddCardViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _addCardModel = new AddCardModel();
            AddNewCardCommand = new Command(AddNewCard);
        }

        private async void AddNewCard()
        {
            // После добавления регистрации должен использовать id конкретного пользователя
            var fakeUserId = 1;
            
            await _addCardModel.AddNewCardAsync(fakeUserId, CardNumber);
            OnNewCardAdded?.Invoke();
            await _navigation.PopAsync();
        }
    }
}