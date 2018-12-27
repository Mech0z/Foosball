using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<bool> AddUser(string email, string username);
        Task<bool> ChangePassword(string email, string newPassword);
        Task<User> GetUser(string email);
        Task<bool> ChangeEmail(string existingEmail, string newEmail);
    }
}