using System;

namespace Foosball.RequestResponse
{
    public class GetStatusResponse
    {
        public bool Activity { get; set; }
        public int Duration { get; set; }
        public DateTime LastActivityStatusSet { get; set; }
    }
}