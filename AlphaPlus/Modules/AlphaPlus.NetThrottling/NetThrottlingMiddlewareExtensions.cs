using Microsoft.AspNetCore.Builder;

namespace AlphaPlus.NetThrottling
{
    public static class NetThrottlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseNetThrottling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<NetThrottlingMiddleware>();
        }
    }
}