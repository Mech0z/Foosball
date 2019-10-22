using System.Collections.Generic;

namespace Models.RequestResponses
{
    public class ChangeUserRolesRequest
    {
        public string? UserEmail { get; set; }
        public List<string>? Roles { get;set; }
    }
}