using System.Collections.Generic;
using Models.Old;

namespace Foosball.Logic
{
    public interface ISeasonLogic
    {
        string StartNewSeason();
        List<Season> GetSeasons();
        Season GetActiveSeason();
    }
}