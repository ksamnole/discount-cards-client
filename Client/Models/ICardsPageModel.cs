using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities;

namespace Client.Models
{
    public interface ICardsPageModel
    {
        Task<IEnumerable<CardEntity>> GetAllUserCardsAsync(int userId);
    }
}