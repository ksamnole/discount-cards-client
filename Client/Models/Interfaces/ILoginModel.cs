using System.Threading.Tasks;
using Client.Entities;

namespace Client.Models.Interfaces
{
    public interface ILoginModel
    {
        Task<bool> Authentication(UserEntity user);
    }
}