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
        public UserRepository(IOptions<MongoDbSettings> settings) : base(settings, "users")
        {
        }

        public async Task<List<User>> GetUsers()
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

        //public void AddUser(User user)
        //{
        //    user.Password = BCrypt.Net.BCrypt.HashPassword("default", BCrypt.Net.BCrypt.GenerateSalt());

        //    Collection.InsertOne(user);
        //}

        //public string Login(User inputUser)
        //{
        //    var user = Collection.AsQueryable().Where(x => x.Email == inputUser.Email).SingleOrDefault();

        //    if (user != null)
        //    {
        //        if (BCrypt.Net.BCrypt.Verify(inputUser.Password, user.Password))
        //        {
        //            return user.Password;
        //        }
        //    }

        //    return string.Empty;
        //}

        //public string ChangePassword(string email, string oldPassword, string newPassword)
        //{
        //    var user = Collection.AsQueryable().Where(x => x.Email == email).SingleOrDefault();

        //    if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
        //    {
        //        return string.Empty;
        //    }

        //    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword, BCrypt.Net.BCrypt.GenerateSalt());

        //    Collection.ReplaceOne(item => item.Id == user.Id, user,
        //                    new UpdateOptions { IsUpsert = true });

        //    return user.Password;
        //}

        //public bool Validate(User inputUser)
        //{
        //    var user = Collection.AsQueryable().Where(x => x.Email == inputUser.Email).SingleOrDefault();

        //    if (user != null)
        //    {
        //        if (user.Password == inputUser.Password)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public User GetUser(string email)
        //{
        //    return Collection.AsQueryable().Where(x => x.Email.Value == email).SingleOrDefault();
        //}

        //public void AddUser(User user)
        //{
        //    string email = user.Email.Value;
        //    var mongoUserEmail = new MongoUserEmail(email);
        //    mongoUserEmail.SetNormalizedEmail(email.ToUpper());
        //    user.SetEmail(mongoUserEmail);

        //    user.GravatarEmail = email;
        //    user.SetNormalizedUserName(email.ToUpper());

        //    Collection.InsertOne(user);
        //}
    }
}