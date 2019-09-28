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

            var currentSeason = HelperMethods.GetCurrentSeason(seasons);
            
            if (currentSeason != null)
            {
                if (currentSeason.StartDate.Date.AddDays(-1).Date == DateTime.UtcNow.Date)
                {
                    throw new Exception("Cant start new season yet");
                }
            }

            var newSeason = new Season
            {
                StartDate = currentSeason != null ? DateTime.UtcNow.Date.AddDays(1) : DateTime.UtcNow.Date,
                Name = $"Season {newSeasonNumber}"
            };

            await _seasonRepository.CreateNewSeason(newSeason);

            return newSeason.Name;
        }

        public async Task<List<Season>> GetSeasons()
        {
            return await _seasonRepository.GetSeasons();
        }

        public async Task<List<Season>> GetStartedSeasons()
        {
            return await _seasonRepository.GetStartedSeasons();
        }

        public async Task<Season> GetActiveSeason()
        {
            var seasons = await _seasonRepository.GetSeasons();

            return HelperMethods.GetCurrentSeason(seasons);
        }
    }
}