using System.Threading.Tasks;

namespace Client.Models
{
    public interface IAddCardModel
    {
        Task AddNewCardAsync(string number);
    }
}