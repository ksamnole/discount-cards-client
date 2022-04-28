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
                new CardEntity() { Name = "Адидас", ImageSource = "adidas.png", Number = "1234567876976" },
                new CardEntity() { Name = "Перекресток", ImageSource = "perekrestok.png", Number = "2432432453243" },
                new CardEntity() { Name = "Магнит", ImageSource = "magnit.png", Number = "3432432432" },
                new CardEntity() { Name = "Спортмастер", ImageSource = "sportmaster.png", Number = "4432432432" },
            };
        }
    }
}