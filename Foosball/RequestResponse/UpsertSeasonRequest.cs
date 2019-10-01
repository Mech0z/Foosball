using System;

namespace Foosball.RequestResponse
{
    public class UpsertSeasonRequest
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}
