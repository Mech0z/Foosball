using System.Threading.Tasks;

namespace Foosball.Hubs
{
    public interface IMatchAddedHub
    {
        Task SendAsync(string title, string name, string message);
    }
}