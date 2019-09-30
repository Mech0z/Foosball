using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Foosball.Hubs
{
    public class MatchAddedHub : Hub, IMatchAddedHub
    {
        public async Task SendAsync(string bla, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, message);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }

    public class ActivitySensorHub : Hub, IActivitySensorHub
    {
        public async Task SendAsync(string title, bool activity)
        {
            await Clients.All.SendAsync("ActivitySensorUpdate", activity);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}