using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Entities.Card;
using Client.Entities.Enums;
using Client.Models.Interfaces;
using Newtonsoft.Json;

namespace Client.Models
{
    public class CardsPageModel : ICardsPageModel
    {
        public async Task<IEnumerable<Card>> GetAllUserCardsAsync(string login)
        {
            var response = await HttpClients.Client.HttpClient.GetAsync($"cards/user/{login}");
            
            var responseBody = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var ex = JsonConvert.DeserializeObject<ValidationException>(responseBody);
                    await UserDialogs.Instance.AlertAsync(ex.Message);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Внутренняя ошибка сервера");
                }

                return null;
            }
            var list =  JsonConvert.DeserializeObject<List<CardEntity>>(responseBody);

            return list.Select(it => new Card()
            {
                Id = it.Id,
                ShopName = (Shops)it.ShopId,
                ImageSource = $"{((Shops)it.ShopId).ToString().ToLower()}.png",
                Number = it.Number
            });
        }
    }
}