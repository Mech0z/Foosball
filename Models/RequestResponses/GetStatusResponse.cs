using System;

namespace Models.RequestResponses
{
    public class GetStatusResponse
    {
        public bool Activity { get; set; }
        public int Duration { get; set; }
        public DateTime LastActivityStatusSet { get; set; }
    }
}