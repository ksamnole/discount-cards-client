using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities;
using Client.Entities.Card;
using Client.Models;
using Client.Models.Interfaces;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class CardPageViewModel : BaseViewModel
    {
        public ICommand DeleteCardAsyncCommand { get; }

        private Card _card { get; }
        private readonly ICardPageModel _cardModel;
        private readonly INavigation _navigation;
        
        public CardPageViewModel(INavigation navigation, Card card)
        {
            _navigation = navigation;
            _card = card;
            _cardModel = new CardPageModel();
            DeleteCardAsyncCommand = new Command(DeleteCardAsync);
        }

        private async void DeleteCardAsync()
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig()
            {
                Message = "Вы точно хотите удалить карту?",
                OkText = "Да",
                CancelText = "Нет"
            });

            if (!result) return;

            await _cardModel.DeleteCardAsync(_card.Id);

            await _navigation.PopAsync();
        }
    }
}