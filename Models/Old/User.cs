using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Old
{
    public class User
    {
        public User(string email, string username)
        {
            Email = email;
            Username = username;
            Roles = new List<string>();
        }

        public Guid Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }

        public string? GravatarEmail { get; set; }

        public List<string>? Roles { get; set; }
    }
}
