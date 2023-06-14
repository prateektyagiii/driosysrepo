using OutReach.CoreAPI.Middlewares;
//using OutReach.CoreAPI.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace OutReach.CoreAPI.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder BearerAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
