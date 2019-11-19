using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Models.Old;
using Models.RequestResponses;
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
        
        public async Task<string> StartNewSeason(UpsertSeasonRequest request)
        {
            var seasons = await _seasonRepository.GetSeasons();

            var existingSeasonOnDate = seasons.SingleOrDefault(x => x.StartDate == request.StartDate);
            if (existingSeasonOnDate != null)
            {
                throw new ArgumentException("Already a season with this date");
            }

            var existingSameName = seasons.SingleOrDefault(x => x.Name == request.Name);
            if (existingSameName != null)
            {
                throw new ArgumentException("Already a season with same name");
            }

            var newSeason = new Season
            {
                StartDate = request.StartDate,
                Name = request.Name
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

        public async Task<Season> GetActiveSeason(List<Season>? seasons = null)
        {
            if (seasons == null)
            {
                seasons = await _seasonRepository.GetSeasons();
            }

            return HelperMethods.GetCurrentSeason(seasons);
        }
    }
}