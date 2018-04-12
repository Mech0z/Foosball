using System.Collections.Generic;
using Models.Old;

namespace Foosball.RequestResponse
{
    public class GetPlayerSeasonHistoryResponse
    {
        public GetPlayerSeasonHistoryResponse()
        {
            PlayerLeaderboardEntries = new List<PlayerLeaderboardEntry>();
        }

        public List<PlayerLeaderboardEntry> PlayerLeaderboardEntries { get; set; }
    }
}