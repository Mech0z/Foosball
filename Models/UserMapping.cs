using System;

namespace Models
{
    public class UserMapping
    {
        public Guid Id { get; set; }
        public string IdentityUsername { get; set; }
        public string NormalUsername { get; set; }
    }
}