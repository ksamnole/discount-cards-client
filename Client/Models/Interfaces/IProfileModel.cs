using System.Threading.Tasks;

namespace Client.Models.Interfaces
{
    public interface IProfileModel
    {
        Task ChangePassword(string login, string currentPassword, string newPassword);
    }
}