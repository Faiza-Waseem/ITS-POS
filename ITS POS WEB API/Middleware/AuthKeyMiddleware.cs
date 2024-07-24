using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace ITS_POS_WEB_API.Middleware
{
    public class AuthKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string AuthKeyName = "AuthKey";
        private const string AuthKeyValue = "DemoTrainingKey";
        private const string BearerPrefix = "Bearer ";

        public AuthKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(AuthKeyName, out var extractedAuthKey) || extractedAuthKey != AuthKeyValue)
            {
                if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader) || !authHeader.First().StartsWith(BearerPrefix))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                var bearerToken = authHeader.First().Substring(BearerPrefix.Length).Trim();

                if (bearerToken == AuthKeyValue)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }

            await _next(context);
        }

    }
}