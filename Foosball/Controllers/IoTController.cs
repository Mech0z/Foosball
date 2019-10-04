using System;
using System.Linq;
using System.Threading.Tasks;
using Foosball.Hubs;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Models.Old;
using Repository;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[EnableCors("CorsPolicy")]
    [ApiController]
    public class IoTController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IActivityRepository _activityRepository;
        private IHubContext<MessageHub, ITypedHubClient> _hubContext;

        public IoTController(IMemoryCache memoryCache,
            IActivityRepository activityRepository,
            IHubContext<MessageHub, ITypedHubClient> hubContext)
        {
            _memoryCache = memoryCache;
            _activityRepository = activityRepository;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<ActionResult> UpdateActivityStatus(UpdateActivityStatusRequest request)
        {
            string retMessage;

            try
            {
                await _hubContext.Clients.All.SendMessageToClient("TestTitle", "activity", "sdfdsfdsf");
                retMessage = "Success";
            }
            catch (Exception e)
            {
                retMessage = e.ToString();
            }

            _memoryCache.Set("ActivityStatus", request.Activity);
            _memoryCache.Set("ActivityDuration", request.Duration);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetStatus()
        {
            _memoryCache.TryGetValue("ActivityStatus", out bool status);
            _memoryCache.TryGetValue("ActivityDuration", out int duration);
            return Ok(new GetStatusResponse
            {
                Activity = status,
                Duration = duration
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateActivity(CreateActivitySummeryRequest request)
        {
            var date = DateTime.UtcNow.Date;
            await _activityRepository.UpsertActivity(new Activity
            {
                Date = date, 
                ActivityEntries = request.ActivitySummeryEntries.Select(x => new ActivityEntry
                {
                    HourIndex = x.HourIndex,
                    Minutes = x.Minutes
                }).ToList()
            });

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetActivity()
        {
            var activites = await _activityRepository.GetActivities();

            return Ok(activites);
        }
    }
}
