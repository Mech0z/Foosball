using System.Threading.Tasks;
using Foosball.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models.RequestResponses;

namespace Foosball.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LiveMatchController : Controller
    {
        private readonly IHubContext<MessageHub, ITypedHubClient> _hubContext;

        public LiveMatchController(IHubContext<MessageHub, ITypedHubClient> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivityStatus(UpdateActivityStatusRequest request)
        {
            await _hubContext.Clients.All.UpdateActivityStatus(request);

            return Ok();
        }
    }
}