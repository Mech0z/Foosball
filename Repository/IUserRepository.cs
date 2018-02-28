using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Old;

namespace Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        //void AddUser(User user);
        //User GetUser(string email);
        //string Login(User inputUser);
        //bool Validate(User inputUser);
        //string ChangePassword(string email, string hashedPassword, string newPassword);
    }
}