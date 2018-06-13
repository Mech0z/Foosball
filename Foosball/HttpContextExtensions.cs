using System.Linq;
using Microsoft.AspNetCore.Http;
using Models;

namespace Foosball
{
    public static class HttpContextExtensions
    {
        public static LoginSession GetLoginSession(this HttpContext context)
        {
            var loginSession = new LoginSession
            {
                Token = context.User.Claims.Single(x => x.Type == "Token").Value,
                Email = context.User.Claims.Single(x => x.Type == "Email").Value,
                DeviceName = context.User.Claims.Single(x => x.Type == "DeviceName").Value
            };

            return loginSession;
        }
    }
}