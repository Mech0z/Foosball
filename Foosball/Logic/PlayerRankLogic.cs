using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Old;
using Repository;

namespace Foosball.Logic
{
    public class PlayerRankLogic : IPlayerRankLogic
    {
        private readonly IPlayerRankHistoryRepository _playerRankHistoryRepository;

        public PlayerRankLogic(IPlayerRankHistoryRepository playerRankHistoryRepository)
        {
            _playerRankHistoryRepository = playerRankHistoryRepository;
        }

        public async Task<PlayerRankSeasonEntry> GetPlayerRankAsync(string email, string seasonName)
        {
            var data = await _playerRankHistoryRepository.GetPlayerRankHistory(email, seasonName);
            return data;
        }

        public async Task<List<PlayerRankSeasonEntry>> GetPlayerRanksAsync(string seasonName)
        {
            var data = await _playerRankHistoryRepository.GetPlayerRankHistories(seasonName);
            var filteredData = GetLastEntryOfEachDay(data);
            var filledOutData = FillOutPlayerRankBlanks(filteredData);
            return filledOutData;
        }

        public List<PlayerRankSeasonEntry> FillOutPlayerRankBlanks(List<PlayerRankSeasonEntry> playerRankSeasonEntries)
        {
            List<PlayerRankPlot> allEntires = playerRankSeasonEntries.SelectMany(x => x.RankPlots).ToList();
            List<DateTime> uniqueDates = allEntires.Select(x => x.Date.Date).Distinct().ToList();

            for (int i = 0; i < playerRankSeasonEntries.Count; i++)
            {
                var entry = playerRankSeasonEntries[i];
                entry.RankPlots = entry.RankPlots.OrderBy(x => x.Date).ToList();
                for (int j = 0; j < uniqueDates.Count; j++)
                {
                    var date = uniqueDates.ElementAt(j);
                    var exists = entry.RankPlots.SingleOrDefault(x => x.Date.Date == date);
                    if (exists == null)
                    {
                        var prev = entry.RankPlots.Where(x => x.Date.Date < date).OrderBy(x => x.Date).LastOrDefault();
                        if (prev == null)
                        {
                            entry.RankPlots.Add(new PlayerRankPlot(date, 0, 1500));
                        }
                        else
                        {
                            entry.RankPlots.Add(new PlayerRankPlot(date, prev.Rank, prev.EloRating));
                        }
                    }
                    else
                    {
                        exists.Date = exists.Date.Date;
                    }
                }

                entry.RankPlots = entry.RankPlots.OrderBy(x => x.Date).ToList();
            }

            return playerRankSeasonEntries;
        }

        public List<PlayerRankSeasonEntry> GetLastEntryOfEachDay(List<PlayerRankSeasonEntry> playerRankSeasonEntries)
        {
            foreach (var playerRankSeasonEntry in playerRankSeasonEntries)
            {
                PlayerRankPlot? prev = null;
                var ordered = playerRankSeasonEntry.RankPlots.OrderByDescending(x => x.Date);
                var plotsToRemove = new List<PlayerRankPlot>();
                for (int i = 0; i < ordered.Count(); i++)
                {
                    var entry = ordered.ElementAt(i);
                    var playerRankPlot = entry;
                    if (prev == null || prev.Date.Date != entry.Date.Date)
                    {
                        prev = playerRankPlot;
                    }
                    else
                    {
                        if (playerRankPlot.Date.Date == prev.Date.Date)
                        {
                            plotsToRemove.Add(playerRankPlot);
                        }
                    }
                }

                foreach (PlayerRankPlot rankPlot in plotsToRemove)
                {
                    playerRankSeasonEntry.RankPlots.Remove(rankPlot);
                }
            }

            return playerRankSeasonEntries;
        }
    }
}
