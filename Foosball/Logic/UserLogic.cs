using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Models.Old;
using Repository;

namespace Foosball.Logic
{
    public class UserLogic : IUserLogic
    {
        private readonly ILeaderboardViewRepository leaderboardViewRepository;

        public UserLogic(ILeaderboardViewRepository leaderboardViewRepository)
        {
            this.leaderboardViewRepository = leaderboardViewRepository;
        }

        public async Task<List<PlayerLeaderboardEntry>> GetPlayerLeaderboardEntries(string email)
        {
            var result = new List<PlayerLeaderboardEntry>();
            var leaderboards = await leaderboardViewRepository.GetLeaderboardViews();

            foreach (var leaderboard in leaderboards)
            {
                var leaderboardEntries = leaderboard.Entries.OrderByDescending(x => x.EloRating).ToList();
                var playerEntry = leaderboardEntries.SingleOrDefault(x => x.UserName == email);
                if (playerEntry != null)
                {
                    result.Add(new PlayerLeaderboardEntry
                    {
                        UserName = email,
                        EloRating = playerEntry.EloRating,
                        Losses = playerEntry.Losses,
                        NumberOfGames = playerEntry.NumberOfGames,
                        Wins = playerEntry.Wins,
                        SeasonName = leaderboard.SeasonName,
                        Rank = leaderboardEntries.IndexOf(playerEntry) + 1
                    });
                }
            }

            return result;
        }
    }
}