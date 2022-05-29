using System.Threading.Tasks;

namespace Client.Models.Interfaces
{
    public interface ICardPageModel
    {
        Task DeleteCardAsync(string number);
    }
}