using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities;
using Client.Entities.Card;

namespace Client.Models.Interfaces
{
    public interface ICardsPageModel
    {
        Task<IEnumerable<Card>> GetAllUserCardsAsync(string login);
    }
}