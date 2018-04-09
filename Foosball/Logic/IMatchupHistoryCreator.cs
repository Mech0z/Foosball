using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Foosball.Logic
{
    public interface IMatchupHistoryCreator : ILogic
    {
        Task<List<PartnerPercentResult>> GetPartnerWinPercent(string email, string season);
        Task AddMatch(Match match);
        void RecalculateMatchupHistory();
    }
}