using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Models
{
    public class CardsPageModel : ICardsPageModel
    {
        public async Task<IEnumerable<CardEntity>> GetAllUserCardsAsync(int userId)
        {
            var response = await HttpClients.Client.HttpClient.GetAsync($"cards/user/{userId}");

            if (!response.IsSuccessStatusCode) throw new Exception("Внутренняя ошибка сервера");
            
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CardEntity>>(responseBody);
        }
    }
}