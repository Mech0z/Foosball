using System;
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

            var newUser = new User(email.ToLowerInvariant(), username);

            await Collection.InsertOneAsync(newUser);

            return true;
        }

        public async Task<User> GetUser(string email)
        {
            return await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> ChangeEmail(string existingEmail, string newEmail)
        {
            var session = Client.StartSession();

            session.StartTransaction();

            try
            {
                var query = Collection.AsQueryable();
                var user = await query.SingleAsync(x => x.Email == existingEmail);

                user.Email = newEmail;

                await Collection.ReplaceOneAsync(item => item.Id == user.Id, user,
                    new UpdateOptions { IsUpsert = false });

                var matchesCollection = Database.GetCollection<Match>("MatchesV3");
                var result = matchesCollection.AsQueryable().Where(x => x.PlayerList.Contains(existingEmail));

                var matches = await result.ToListAsync();

                foreach (var match in matches)
                {
                    for (int i = 0; i < match.PlayerList.Count; i++)
                    {
                        if (match.PlayerList[i] == existingEmail)
                        {
                            match.PlayerList[i] = newEmail;
                        }
                    }
                    await matchesCollection.ReplaceOneAsync(cmatch => cmatch.Id == match.Id, match,
                        new UpdateOptions { IsUpsert = false });
                }

                var userLoginInfoCollection = Database.GetCollection<UserLoginInfo>("UserLoginInfos");
                var existingUserLogin = await userLoginInfoCollection.AsQueryable()
                    .SingleOrDefaultAsync(x => x.Email == existingEmail);

                existingUserLogin.Email = newEmail;

                await userLoginInfoCollection.ReplaceOneAsync(userLogin => userLogin.Id == existingUserLogin.Id,
                    existingUserLogin,
                    new UpdateOptions {IsUpsert = false});

                var leaderboardViewCollection = Database.GetCollection<LeaderboardView>("LeaderboardViews");
                var leaderboardquery = leaderboardViewCollection.AsQueryable();
                var leaderboards = await leaderboardquery.ToListAsync();

                foreach (var leaderboardView in leaderboards)
                {
                    foreach (var entry in leaderboardView.Entries)
                    {
                        if (entry.UserName == existingEmail)
                        {
                            entry.UserName = newEmail;
                        }
                    }

                    await leaderboardViewCollection.ReplaceOneAsync(lbw => lbw.Id == leaderboardView.Id, leaderboardView,
                        new UpdateOptions { IsUpsert = false });
                }

                session.CommitTransaction();

                return true;
            }
            catch (Exception e)
            {
                session.AbortTransaction();

                return false;
            }
        }
    }
}