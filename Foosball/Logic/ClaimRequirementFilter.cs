using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Models;

namespace Foosball.Logic
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, ClaimRoles claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] {new Claim(claimType, claimValue.ToString()) };
        }
    }
    
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            var tokenSuccess = headers.TryGetValue("Token", out var token);

            var emailSuccess = headers.TryGetValue("Email", out var email);

            var deviceNameSuccess = headers.TryGetValue("DeviceName", out var deviceName);

            if (tokenSuccess && emailSuccess && deviceNameSuccess)
            {
                var accountLogic = context.HttpContext.RequestServices.GetService<IAccountLogic>();

                var hasClaim = accountLogic.ValidateLogin(email, token, deviceName).Result.Success;

                if (!hasClaim)
                {
                    context.HttpContext.ForbidAsync();
                }
            }
            else
            {
                context.HttpContext.ForbidAsync();
            }
        }
    }
}