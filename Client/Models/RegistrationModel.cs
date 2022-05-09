using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Models.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Client.Models
{
    public class RegistrationModel : IRegistrationModel
    {
        public async Task<bool> Registration(UserEntity user)
        {
            var requestUri = $"user";
            var data = JsonConvert.SerializeObject(user);
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

                return false;
            }
            Application.Current.Properties.Add("User", user.Login);
            Application.Current.Properties.Add("Password", user.Password);
                
            await Application.Current.SavePropertiesAsync();

            return true;
        }
    }
}