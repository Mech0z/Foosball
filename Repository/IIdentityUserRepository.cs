using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface IIdentityUserRepository
    {
        Task<List<string>> GetIdentityEmails();
    }
}