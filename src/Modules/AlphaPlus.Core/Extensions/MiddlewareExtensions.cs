using Microsoft.AspNetCore.Builder;
using AlphaPlus.Core.Middlewares;

namespace AlphaPlus.Core.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseSessionCookieComparison(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SessionCookieComparisonMiddleware>();
    }
} 