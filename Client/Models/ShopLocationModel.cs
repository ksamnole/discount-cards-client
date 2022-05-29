using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Entities.Card;
using Client.Entities.ShopLocation;
using Newtonsoft.Json;

namespace Client.Models
{
    public static class ShopLocationModel
    {
        public static async Task UpdateDistanceOnCards(float longtitude, float latitude, IEnumerable<Card> cards)
        {
            // var shopLocations = new List<ShopLocation>();
            var shopLocations = new Dictionary<Card, double>();
            
            var requestUri = $"shoplocations";

            foreach (var card in cards)
            {
                if (card.ShopName == "Другое")
                    continue;

                var data = JsonConvert.SerializeObject(new RequestShopLocation()
                {
                    Longtitude = longtitude,
                    Latitude = latitude,
                    ShopName = card.ShopName
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
                
                var tLongtitude = Math.Abs(longtitude - shopLocation.Longtitude);
                var tLatitude = Math.Abs(latitude - shopLocation.Latitude);
                
                var path = Math.Sqrt(Math.Pow(tLatitude, 2) * Math.Pow(tLongtitude, 2));

                card.LastDistanceToShop = path;

                await App.CardDb.UpdateCard(card);

                // shopLocations.Add(card, path);

                // var shopLocation = JsonConvert.DeserializeObject<ShopLocation>(responseBody);
                //
                // shopLocation.CardId = card.Id;
                //
                // shopLocations.Add(shopLocation);
            }

            // return shopLocations.OrderBy(x => x.Value).Select(x => x.Key).ToList();

            // var nearestCardId = -999;
            // var nearestPath = double.MaxValue;
            //
            // foreach (var t in shopLocations)
            // {
            //     var tLongtitude = Math.Abs(longtitude - t.Longtitude);
            //     var tLatitude = Math.Abs(latitude - t.Latitude);
            //
            //     var path = Math.Sqrt(Math.Pow(tLatitude, 2) * Math.Pow(tLongtitude, 2));
            //
            //     if (path < nearestPath)
            //     {
            //         nearestPath = path;
            //         nearestCardId = t.CardId;
            //     }
            // }
            //
            // return nearestCardId;
        }
    }
}