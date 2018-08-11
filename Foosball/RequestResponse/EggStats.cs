using System.Collections.Generic;
using Models.Old;

namespace Foosball.RequestResponse
{
    public class EggStats
    {
        public EggStats()
        {
            MatchesReceivedEgg = new List<Match>();
            MatchesGivenEgg = new List<Match>();
        }

        public List<Match> MatchesReceivedEgg { get; set; }
        public List<Match> MatchesGivenEgg { get; set; }
    }
}
