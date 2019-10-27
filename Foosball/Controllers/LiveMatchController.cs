using System;
using System.Threading.Tasks;
using Foosball.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Models.RequestResponses;

namespace Foosball.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LiveMatchController : Controller
    {
        private readonly IHubContext<MessageHub, ITypedHubClient> _hubContext;
        private readonly IMemoryCache _memoryCache;
        private string _liveMatchUpdateRequest = "LiveMatchUpdateRequest";

        public LiveMatchController(IHubContext<MessageHub, ITypedHubClient> hubContext, IMemoryCache memoryCache)
        {
            _hubContext = hubContext;
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivityStatus(LiveMatchUpdateRequest request)
        {
            try
            {
                await _hubContext.Clients.All.UpdateActivityStatus(request);
            }
            catch (Exception e)
            {
            }

            _memoryCache.Set(_liveMatchUpdateRequest, request);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUpdateActivityStatus()
        {
            _memoryCache.TryGetValue(_liveMatchUpdateRequest, out LiveMatchUpdateRequest liveMatchUpdateRequest);
            return Ok(liveMatchUpdateRequest);
        }
    }
}