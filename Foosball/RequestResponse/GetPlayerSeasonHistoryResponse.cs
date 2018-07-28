using System.Collections.Generic;
using Models.Old;

namespace Foosball.RequestResponse
{
    public class GetPlayerSeasonHistoryResponse
    {
        public GetPlayerSeasonHistoryResponse()
        {
            PlayerLeaderBoardEntries = new List<PlayerLeaderboardEntry>();
        }

        public List<PlayerLeaderboardEntry> PlayerLeaderBoardEntries { get; set; }
    }
}