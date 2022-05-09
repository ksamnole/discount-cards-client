using System;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Models.Interfaces;
using Newtonsoft.Json;

namespace Client.Models
{
    public class CardPageModel : ICardPageModel
    {
        public async Task DeleteCardAsync(int id)
        {
            var response = await HttpClients.Client.HttpClient.DeleteAsync($"cards/{id}");

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
            }
        }
    }
}