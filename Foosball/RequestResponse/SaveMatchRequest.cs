using System.Collections.Generic;
using Models.Old;

namespace Foosball.RequestResponse
{
    public class SaveMatchesRequest
    {
        public User User { get; set; }
        public List<Match> Matches { get; set; }
    }
}