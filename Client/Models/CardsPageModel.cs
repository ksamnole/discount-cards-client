using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities;

namespace Client.Models
{
    public class CardsPageModel : ICardsPageModel
    {
        public async Task<IEnumerable<CardEntity>> GetAllCardsAsync()
        {
            return new List<CardEntity>()
            {
                new CardEntity() { Name = "adidas", ImageSource = "adidas.png", Number = "1" },
                new CardEntity() { Name = "perekrestok", ImageSource = "perekrestok.png", Number = "2" },
                new CardEntity() { Name = "magnit", ImageSource = "magnit.png", Number = "3" },
                new CardEntity() { Name = "sportmaster", ImageSource = "sportmaster.png", Number = "4" },
            };
        }
    }
}