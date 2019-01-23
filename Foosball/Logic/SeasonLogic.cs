using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Old;
using Repository;

namespace Foosball.Logic
{
    public class SeasonLogic : ISeasonLogic
    {
        private readonly ISeasonRepository _seasonRepository;

        public SeasonLogic(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }
        
        public async Task<string> StartNewSeason()
        {
            var seasons = await _seasonRepository.GetSeasons();
            var newSeasonNumber = seasons.Count + 1;

            var activeSeason = seasons.SingleOrDefault(x => x.EndDate == null);
            
            if (activeSeason != null)
            {
                if (activeSeason.StartDate.Date.AddDays(-1).Date == DateTime.UtcNow.Date)
                {
                    throw new Exception("Cant start new season yet");
                }

                await _seasonRepository.EndSeason(activeSeason);
            }

            var newSeason = new Season
            {
                StartDate = activeSeason != null ? DateTime.UtcNow.Date.AddDays(1) : DateTime.UtcNow.Date,
                Name = $"Season {newSeasonNumber}"
            };

            await _seasonRepository.CreateNewSeason(newSeason);

            return newSeason.Name;
        }

        public async Task<List<Season>> GetSeasons()
        {
            return await _seasonRepository.GetSeasons();
        }

        public async Task<Season> GetActiveSeason()
        {
            var existingSeasons = await _seasonRepository.GetSeasons();

            return existingSeasons.SingleOrDefault(x => x.EndDate == null);
        }
    }
}