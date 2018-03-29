using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FoosballCore.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var contains = context.Request.Headers.Keys.Contains("keytoken");
            //if (contains)
            //{
            //    context.Request.Headers.Keys[];


            //}

            await _next.Invoke(context);
        }
    }
}
