using System.Threading.Tasks;
using Client.Entities;

namespace Client.Models.Interfaces
{
    public interface IRegistrationModel
    {
        Task<bool> Registration(UserEntity user);
    }
}