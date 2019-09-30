using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Foosball.Hubs
{
    public class MessageHub : Hub<ITypedHubClient>
    {
        public async Task SendMessage(string title, string user, string message)
        {
            await Clients.All.SendMessageToClient(title, user, message);
        }
    }
}