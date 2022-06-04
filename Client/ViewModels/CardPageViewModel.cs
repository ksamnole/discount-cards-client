using System;
using System.Collections.Generic;
using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities.Card;
using Client.Models;
using Client.Models.Interfaces;
using Client.Views;
using Xamarin.Essentials;
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

            await App.CardDb.DeleteCardByNumber(_card.Number);

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await _cardModel.DeleteCardAsync(_card.Number);
            }
            else
            {
                if (!Application.Current.Properties.ContainsKey("NeedSync"))
                    Application.Current.Properties.Add("NeedSync", 1);

                await App.DeletedCardDb.AddAsync(_card);
            }

            CardsPage.NeedRefresh = true;

            await _navigation.PopAsync();
        }
    }
}