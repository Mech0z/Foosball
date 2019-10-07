using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public interface IMatchupHistoryCreator : ILogic
    {
        Task<List<PartnerPercentResult>> GetPartnerWinPercent(List<Season> seasons, string email, Season season);
        Task AddMatch(Match match);
        void RecalculateMatchupHistory();
    }
}