using System;
using System.Collections.Generic;
using System.Windows.Input;
using Acr.UserDialogs;
using Client.Entities;
using Client.Entities.Card;
using Client.Models;
using Client.Models.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Client.ViewModels
{
    public class CardPageViewModel : BaseViewModel
    {
        public ICommand DeleteCardAsyncCommand { get; }
        public static List<string> NotActiveCardNumbers { get; set; }
        private Card _card { get; }
        private readonly ICardPageModel _cardModel;
        private readonly INavigation _navigation;
        
        public CardPageViewModel(INavigation navigation, Card card)
        {
            _navigation = navigation;
            _card = card;
            _cardModel = new CardPageModel();
            DeleteCardAsyncCommand = new Command(DeleteCardAsync);
            NotActiveCardNumbers = new List<string>();
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

                NotActiveCardNumbers.Add(_card.Number);
            }

            await _navigation.PopAsync();
        }
    }
}