using System.Threading.Tasks;

namespace Foosball.Hubs
{
    public interface ITypedHubClient
    {
        Task SendMessageToClient(string title, string name, string message);
    }
}