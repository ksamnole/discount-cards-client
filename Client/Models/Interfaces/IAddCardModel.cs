﻿using System.Threading.Tasks;
using Client.Entities;
using Client.Entities.Card;

namespace Client.Models.Interfaces
{
    public interface IAddCardModel
    {
        Task AddNewCardAsync(CreateCardEntity card);
    }
}