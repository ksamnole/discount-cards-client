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
        private readonly string _login;

        public AddCardViewModel(INavigation navigation, string login)
        {
            _login = login;
            _navigation = navigation;
            _addCardModel = new AddCardModel();
            AddNewCardCommand = new Command(AddNewCard);
        }

        private async void AddNewCard()
        {
            if (string.IsNullOrEmpty(CardNumber))
            {
                await UserDialogs.Instance.AlertAsync("Номер карты не должен быть пустым");
                return;
            }
            
            await _addCardModel.AddNewCardAsync(new CreateCardEntity()
            {
                UserLogin = _login,
                ShopId = int.Parse(CardNumber[0].ToString()),
                Number = CardNumber
            });
            
            OnNewCardAdded?.Invoke();
            
            await _navigation.PopAsync();
        }
    }
}