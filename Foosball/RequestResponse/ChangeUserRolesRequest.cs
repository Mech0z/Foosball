using System.Collections.Generic;

namespace Foosball.RequestResponse
{
    public class ChangeUserRolesRequest : BaseRequest
    {
        public string UserEmail { get; set; }
        public List<string> Roles { get;set; }
    }
}