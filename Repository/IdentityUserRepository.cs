using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public class IdentityUserRepository : BaseRepository<IdentityUser>, IIdentityUserRepository
    {
        public IdentityUserRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "users")
        {
        }

        public async Task<List<string>> GetIdentityEmails()
        {
            var emails = await Collection.AsQueryable().Select(x => x.Email).ToListAsync();
            return emails;
        }
    }
}