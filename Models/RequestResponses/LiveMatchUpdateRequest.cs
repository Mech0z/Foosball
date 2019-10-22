using System;
using System.Collections.Generic;

namespace Models.RequestResponses
{
    public class LiveMatchUpdateRequest
    {
        public LiveMatchUpdateRequest()
        {
            Team1Players = new List<string>();
            Team2Players = new List<string>();
            LiveMatchUpdates = new List<LiveMatchUpdate>();
        }

        public DateTime StartTime { get; set; }
        public List<string> Team1Players { get; set; }
        public List<string> Team2Players { get; set; }
        public List<LiveMatchUpdate> LiveMatchUpdates { get; set; }
    }

    public class LiveMatchUpdate
    {
        public DateTime Timestamp { get; set; }
        public EventType EventType { get; set; }
        public int Team { get; set; }
    }

    public enum EventType
    {
        Goal = 1,
        OwnGoal = 2,
        PlayerSwitch = 3
    }

    
}