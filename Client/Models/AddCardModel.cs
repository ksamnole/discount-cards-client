using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Client.Entities;
using Newtonsoft.Json;

namespace Client.Models
{
    public class AddCardModel : IAddCardModel
    {
        public async Task AddNewCardAsync(int userId, string number)
        {
            var requestUri = $"cards";
            var data = JsonConvert.SerializeObject(new CardEntity()
            {
                Name = "Perekrestok",
                ImageSource = "perekrestok.png",
                Number = number,
                UserId = userId
            });
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            var response = await HttpClients.Client.HttpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Внутренняя ошибка сервера");
        }
    }
}