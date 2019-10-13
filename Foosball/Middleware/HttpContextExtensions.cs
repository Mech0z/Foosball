using System.Linq;
using Microsoft.AspNetCore.Http;
using Models;

namespace Foosball.Middleware
{
    public static class HttpContextExtensions
    {
        public static LoginSession GetLoginSession(this HttpContext context)
        {
            var loginSession = new LoginSession(context.User.Claims.Single(x => x.Type == "Token").Value,
                context.User.Claims.Single(x => x.Type == "Email").Value,
                context.User.Claims.Single(x => x.Type == "DeviceName").Value);

            return loginSession;
        }
    }
}