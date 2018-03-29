using System.Collections.Generic;

namespace Models.Old
{
    public class UserRole
    {
        public UserRole(UserLoginInfo userLoginInfo)
        {
            Email = userLoginInfo.Email;
            Roles = userLoginInfo.Roles;
        }

        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}