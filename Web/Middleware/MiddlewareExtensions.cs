using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Web.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
