using System.Collections.Generic;
using Models.Old;

namespace Models.RequestResponses
{
    public class GetPlayerSeasonHistoryResponse
    {
        public GetPlayerSeasonHistoryResponse()
        {
            PlayerLeaderBoardEntries = new List<PlayerLeaderboardEntry>();
        }

        public List<PlayerLeaderboardEntry> PlayerLeaderBoardEntries { get; set; }
        public EggStats EggStats { get; set; }
    }
}