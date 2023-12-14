using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Helpers;

public static class AlphaContext
{
    private static IHttpContextAccessor _httpContextAccessor;

    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public static HttpContext Current => _httpContextAccessor.HttpContext;
}
