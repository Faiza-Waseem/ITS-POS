using Microsoft.AspNetCore.Authorization;
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

        public AuthKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint != null && endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(AuthKeyName, out var extractedAuthKey) || extractedAuthKey != AuthKeyValue)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }

    }
}