using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Foosball.Hubs
{
    public class MatchAddedHub : Hub
    {
        public async Task SendAsync2(string bla, string user, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", message, message);
        }



        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }

    public interface IMatchAddedHub
    {
        Task SendAsync(string title, string name, string message);
    }
    
    public class MessageHub : Hub<ITypedHubClient>
    {
        public async Task SendMessage(string title, string user, string message)
        {
            await Clients.All.SendMessageToClient(title, user, message);
        }
    }
    public interface ITypedHubClient
    {
        Task SendMessageToClient(string title, string name, string message);
    }
}