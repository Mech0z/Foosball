using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Foosball.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var statusCodeFeature = context.Features.Get<IStatusCodePagesFeature>();
                    if (statusCodeFeature == null || !statusCodeFeature.Enabled)
                    {
                        // there's no StatusCodePagesMiddleware in app
                        if (!context.Response.HasStarted)
                        {

                        }
                    }
                }
            }
            catch (LoginExpiredException)
            {
                context.Response.StatusCode = 419;
            }
            catch (Exception ex)
            {
            }
        }
    }
}