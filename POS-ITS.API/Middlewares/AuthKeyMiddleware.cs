using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace POS_ITS.API.Middlewares
{
    public class AuthKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string AuthKeyName = "AuthKey";
        private const string AuthKeyValue = "DemoTrainingKey";
        private const string AuthTokenName = "Authorization";

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

            if (!context.Request.Headers.TryGetValue(AuthKeyName, out var extractedKey))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Bad Request: Missing required header.");
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
