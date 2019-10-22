using System.Collections.Generic;

namespace Models.RequestResponses
{
    public class CreateActivitySummeryRequest
    {
        public List<ActivitySummeryEntry>? ActivitySummeryEntries { get; set; }
    }
}