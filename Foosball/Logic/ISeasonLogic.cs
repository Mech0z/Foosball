using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public interface ISeasonLogic
    {
        Task<string> StartNewSeason();
        Task<List<Season>> GetSeasons();
        Task<Season> GetActiveSeason();
    }
}