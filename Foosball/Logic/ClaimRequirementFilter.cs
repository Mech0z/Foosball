﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using Foosball.Middleware;
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
        private readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers;

            var tokenSuccess = headers.TryGetValue("Token", out var token);
            if(!tokenSuccess) throw new AccessViolationException("Invalid 'Token' Header");

            var emailSuccess = headers.TryGetValue("Email", out var email);
            if (!emailSuccess) throw new AccessViolationException("Invalid 'Email' Header");

            var deviceNameSuccess = headers.TryGetValue("DeviceName", out var deviceName);
            if (!deviceNameSuccess) throw new AccessViolationException("Invalid 'DeviceName' Header");
            
            var accountLogic = context.HttpContext.RequestServices.GetService<IAccountLogic>();
            bool hasClaim;
            if (_claim.Value == ClaimRoles.Unauth.ToString())
            {
                context.HttpContext.User.AddIdentity(
                    new ClaimsIdentity(new List<Claim> { new Claim("Role", _claim.Value) }));

                hasClaim = true;
            }
            else
            {
                var validateLoginResult = accountLogic.ValidateLogin(email, token, deviceName, _claim.Value).Result;

                if (validateLoginResult.Expired)
                {
                    throw new LoginExpiredException();
                }

                
                if (validateLoginResult.Roles.Contains(ClaimRoles.Admin.ToString()))
                {
                    context.HttpContext.User.AddIdentity(
                        new ClaimsIdentity(new List<Claim> { new Claim("Role", ClaimRoles.Admin.ToString()) }));
                    validateLoginResult.Success = true;
                }

                if (validateLoginResult.Success)
                {
                    context.HttpContext.User.AddIdentity(
                        new ClaimsIdentity(new List<Claim> {new Claim("Role", _claim.Value)}));
                }

                hasClaim = validateLoginResult.Success;
            }

            if (!hasClaim && _claim.Value != ClaimRoles.Admin.ToString())
            {
                hasClaim = accountLogic.ValidateLogin(email, token, deviceName, ClaimRoles.Admin.ToString()).Result.Success;
                if (hasClaim)
                {
                    context.HttpContext.User.AddIdentity(
                        new ClaimsIdentity(new List<Claim> { new Claim("Role", ClaimRoles.Admin.ToString())}));
                }
            }
            
            if (!hasClaim)
            {
                throw new AccessViolationException($"Missing Role '{_claim.Value}' on current user");
            }

            context.HttpContext.User.AddIdentity(
                new ClaimsIdentity(new List<Claim> { new Claim("Email", email) }));

            context.HttpContext.User.AddIdentity(
                new ClaimsIdentity(new List<Claim> { new Claim("Token", token) }));

            context.HttpContext.User.AddIdentity(
                new ClaimsIdentity(new List<Claim> { new Claim("DeviceName", deviceName) }));
        }
    }
}