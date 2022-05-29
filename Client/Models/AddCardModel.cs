using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Entities.Card;
using Client.Entities.Shop;
using Client.Models.Interfaces;
using Newtonsoft.Json;

namespace Client.Models
{
    public class AddCardModel : IAddCardModel
    {
        public async Task<int> AddNewCardAsync(CreateCardEntity card)
        {
            var requestUri = $"cards";
            var data = JsonConvert.SerializeObject(card);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await HttpClients.Client.HttpClient.PostAsync(requestUri, content);

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

                return -1;
            }

            return int.Parse(responseBody);
        }

        public async Task<IEnumerable<Shop>> GetShopsAsync()
        {
            var response = await HttpClients.Client.HttpClient.GetAsync($"shop");

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
            
            var list = JsonConvert.DeserializeObject<List<Shop>>(responseBody);

            return list.Select(it => new Shop()
            {
                Id = it.Id,
                Name = it.Name,
            });
        }
    }
}