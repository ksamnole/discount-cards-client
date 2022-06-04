using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Entities.Card;
using Client.Entities.ShopLocation;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Client.Models
{
    public static class ShopLocationModel
    {
        public static async Task UpdateDistanceOnCards(double longitude, double latitude, IEnumerable<Card> cards)
        {
            var requestUri = $"shoplocations";

            foreach (var card in cards)
            {
                var data = JsonConvert.SerializeObject(new RequestShopLocation()
                {
                    Longitude = longitude,
                    Latitude = latitude,
                    Shop = card.ShopName
                });
                
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

                    return;
                }
                
                var shopLocation = JsonConvert.DeserializeObject<ShopLocation>(responseBody);

                var distance =
                    Location.CalculateDistance(latitude,longitude,shopLocation.Latitude, shopLocation.Longitude, DistanceUnits.Kilometers);

                card.LastDistanceToShop = distance;

                await App.CardDb.UpdateCard(card);
            }
        }
    }
}