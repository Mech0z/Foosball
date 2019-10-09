using System;
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

        public async Task SendMatchAddedMessage()
        {
            await Clients.All.MatchAdded();
        }

        public async Task ActivityUpdatedMessage(bool activity, int duration, DateTime lastUpdatedTime)
        {
            await Clients.All.ActivityUpdated(activity, duration, lastUpdatedTime);
        }
    }
}