using System;
using Models.Old;

namespace Foosball.RequestResponse
{
    public class UserResponse
    {
        public UserResponse(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.Username;
            GravatarEmail = user.GravatarEmail;
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string GravatarEmail { get; set; }
    }
}