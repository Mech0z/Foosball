using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using Models.Old;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Repository
{
    public interface IUserLoginInfoRepository
    {
        Task<LoginResult> VerifyLogin(string email, string token, string deviceName);
        Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName);
        bool Logout(string email, string token, string deviceName);
        LoginToken ChangePassword(string email, string token, string deviceName, string newPassword);
        LoginToken CreateUser(string email, string token, string deviceName);
    }

    public class UserLoginInfoRepository : BaseRepository<UserLoginInfo>, IUserLoginInfoRepository
    {
        public UserLoginInfoRepository(IOptions<ConnectionStringsSettings> settings) : base(settings, "UserLoginInfos")
        {
        }


        public async Task<LoginResult> VerifyLogin(string email, string token, string deviceName)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);

            var loginToken =
                existingUserLogin?.Tokens.SingleOrDefault(x => x.DeviceName == deviceName && x.Token == token);

            if (loginToken == null || loginToken.Expirytime < DateTime.Now) return new LoginResult();

            loginToken.Expirytime = DateTime.Now.AddDays(14);
            
            await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);
            
            return new LoginResult{Success = true, LoginToken = loginToken};
        }

        public async Task<LoginResult> Login(string email, string password, bool rememberMe, string deviceName)
        {
            var existingUserLogin = await Collection.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);
            var correctPassword = BCrypt.Net.BCrypt.Verify(password, existingUserLogin.HashedPassword);

            if (!correctPassword) { return new LoginResult(); }
            
            var tokenGuid = Guid.NewGuid();
            var existingToken = existingUserLogin.Tokens.SingleOrDefault(x => x.DeviceName == deviceName);
            
            var expirytime = rememberMe ? DateTime.Now.AddDays(14) : DateTime.Now.AddHours(1);
            
            if (existingToken != null)
            {
                existingToken.Expirytime = expirytime;
                existingToken.Token = tokenGuid.ToString();
            }
            else
            {
                existingUserLogin.Tokens.Add(new LoginToken
                {
                    Token = tokenGuid.ToString(),
                    DeviceName = deviceName,
                    Expirytime = expirytime
                });
            }
            
            await Collection.ReplaceOneAsync(i => i.Id == existingUserLogin.Id, existingUserLogin);

            var newToken = existingUserLogin.Tokens.SingleOrDefault(x => x.Token == tokenGuid.ToString());
            
            return new LoginResult {Success = true, LoginToken = newToken};
        }

        public bool Logout(string email, string token, string deviceName)
        {
            throw new NotImplementedException();
        }

        public LoginToken ChangePassword(string email, string token, string deviceName, string newPassword)
        {
            throw new NotImplementedException();
        }

        public LoginToken CreateUser(string email, string token, string deviceName)
        {
            throw new NotImplementedException();
        }
    }
}