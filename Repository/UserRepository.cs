using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "Users")
        {
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var query = Collection.AsQueryable();
            var users = await query.ToListAsync();

            foreach (User user in users)
            {
                user.Password = "";
                user.Roles = null;
            }

            return users;
        }

        public async Task<bool> AddUser(string email, string username)
        {
            var query = Collection.AsQueryable();
            var existingResult = await query.SingleOrDefaultAsync(x => x.Email == email || x.Username == username);

            if (existingResult != null)
                return false;

            var newUser = new User
            {
                Email = email,
                Username = username
            };

            await Collection.InsertOneAsync(newUser);

            return true;
        }

        public async Task<bool> ChangePassword(string email, string newPassword)
        {
            var query = Collection.AsQueryable();
            var user = await query.SingleAsync(x => x.Email == email);

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt());

            Collection.ReplaceOne(item => item.Id == user.Id, user,
                            new UpdateOptions { IsUpsert = true });

            return true;
        }

        public async Task<User> GetUser(string email)
        {
            return await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}