using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Models.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Client.Models
{
    public class LoginModel : ILoginModel
    {
        public async Task<bool> Authentication(UserEntity user)
        {
            var requestUri = $"user/auth";
            var data = JsonConvert.SerializeObject(new
            {
                Login = user.Login,
                Password = user.Password
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

                return false;
            }

            if (!Convert.ToBoolean(responseBody)) return false;
            
            Application.Current.Properties.Add("User", user.Login);
            Application.Current.Properties.Add("Password", user.Password);

            await Application.Current.SavePropertiesAsync();

            return true;
        }
    }
}