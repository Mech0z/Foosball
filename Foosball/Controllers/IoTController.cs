using System;
using System.Linq;
using System.Threading.Tasks;
using Foosball.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Models.Old;
using Models.RequestResponses;
using Repository;

namespace Foosball.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IoTController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IActivityRepository _activityRepository;
        private readonly IHubContext<MessageHub, ITypedHubClient> _hubContext;

        private string _activitystatus = "ActivityStatus";
        private string _activityduration = "ActivityDuration";
        private string _lastactivitystatusset = "LastActivityStatusSet";

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
            _memoryCache.TryGetValue(_activitystatus, out bool status);
            if (request.Activity != status)
            {
                _memoryCache.Set(_lastactivitystatusset, DateTime.Now);
            }

            _memoryCache.Set(_activitystatus, request.Activity);
            _memoryCache.Set(_activityduration, request.Duration);

            try
            {
                _memoryCache.TryGetValue(_lastactivitystatusset, out DateTime? lastActivityStatusSet);
                await _hubContext.Clients.All.ActivityUpdated(request.Activity, request.Duration, lastActivityStatusSet);
            }
            catch (Exception)
            {

            }

            return Ok();
        }

        [HttpGet]
        public ActionResult GetStatus()
        {
            _memoryCache.TryGetValue(_activitystatus, out bool status);
            _memoryCache.TryGetValue(_activityduration, out int duration);
            _memoryCache.TryGetValue(_lastactivitystatusset, out DateTime lastActivityStatusSet);

            return Ok(new GetStatusResponse
            {
                Activity = status,
                Duration = duration,
                LastActivityStatusSet = lastActivityStatusSet
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateActivity(CreateActivitySummeryRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

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
