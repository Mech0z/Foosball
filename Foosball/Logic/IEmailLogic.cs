using System.Threading.Tasks;

namespace Foosball.Logic
{
    public interface IEmailLogic
    {
        Task<bool> SendEmail(string email, string subject, string htmlContent);
    }
}