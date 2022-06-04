using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Client.Entities;
using Client.Models.Interfaces;
using Newtonsoft.Json;

namespace Client.Models
{
    public class ProfileModel : IProfileModel
    {
        public async Task ChangePassword(string login, string currentPassword, string newPassword)
        {
            var requestUri = $"user/change_password";
            var data = JsonConvert.SerializeObject(new ChangePassword()
            {
                Login = login,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
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
            
            await UserDialogs.Instance.AlertAsync("Пароль успешно изменен");
        }
    }
}