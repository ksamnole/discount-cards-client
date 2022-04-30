﻿using System;
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
        public ICommand GetAllCardsCommand { get; }
        public ObservableCollection<CardEntity> Cards { get; }
        public event Action OnRefreshCardsCompleted;
 
        private readonly ICardsPageModel _cardsModel;

        public CardsPageViewModel()
        {
            _cardsModel = new CardsPageModel();
            Cards = new ObservableCollection<CardEntity>();
            GetAllCardsCommand = new Command(GetAllCardsAsync);
        }

        public async void GetAllCardsAsync()
        {
            var allCards = await _cardsModel.GetAllCardsAsync();

            Cards.Clear();

            foreach (var card in allCards)
                Cards.Add(card);
            
            OnRefreshCardsCompleted?.Invoke();
        }
    }
}