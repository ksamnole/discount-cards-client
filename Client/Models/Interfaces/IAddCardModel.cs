using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities.Card;
using Client.Entities.Shop;

namespace Client.Models.Interfaces
{
    public interface IAddCardModel
    {
        Task<int> AddNewCardAsync(CreateCardEntity card);
        Task<IEnumerable<Shop>> GetShopsAsync();
    }
}