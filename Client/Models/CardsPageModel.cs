using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities;
using Client.Entities.Database;
using Newtonsoft.Json;

namespace Client.Models
{
    public class CardsPageModel : ICardsPageModel
    {
        public async Task<IEnumerable<CardEntity>> GetAllCardsAsync()
        {
            var response = await HttpClients.Client.HttpClient.GetAsync("cards/1");

            if (!response.IsSuccessStatusCode) throw new Exception("Внутренняя ошибка сервера");
            
            var responseContent = response.Content;
            var cardDto = JsonConvert.DeserializeObject<CardDto>(await responseContent.ReadAsStringAsync());
            
            return new List<CardEntity>()
            {
                new CardEntity() { Name = "Adidas", ImageSource = "adidas.png", Number = cardDto.Number }
            };

        }
    }
}