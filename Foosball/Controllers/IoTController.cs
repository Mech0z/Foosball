using System;
using System.Linq;
using System.Threading.Tasks;
using Foosball.Hubs;
using Foosball.RequestResponse;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models.Old;
using Repository;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class IoTController : Controller
    {
        private readonly IActivitySensorHub _activitySensorHub;
        private readonly IMemoryCache _memoryCache;
        private readonly IActivityRepository _activityRepository;

        public IoTController(IActivitySensorHub activitySensorHub, IMemoryCache memoryCache, IActivityRepository activityRepository)
        {
            _activitySensorHub = activitySensorHub;
            _memoryCache = memoryCache;
            _activityRepository = activityRepository;
        }

        [HttpPost]
        public async Task<ActionResult> UpdateActivityStatus(UpdateActivityStatusRequest request)
        {
            await _activitySensorHub.SendAsync("SensorActivity", request.Activity);
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
