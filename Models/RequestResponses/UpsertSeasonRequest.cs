using System;

namespace Models.RequestResponses
{
    public class UpsertSeasonRequest
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
    }
}
