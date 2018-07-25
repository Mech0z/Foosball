using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task AddUser(string email, string username, string password);
        Task ChangePassword(string email, string newPassword);
        Task<User> GetUser(string email);
    }
}