using AlphaPlus.NetThrottling.Models;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Utils
{
    public static class Extensions
    {
        public static bool IsMatchRequest(this HttpRequest request, ThrottlePolicy key)
        {
            var isPathMatched = key.Path == "*" || key.Path == request.Path.Value;
            var isMethodMatched = key.Method == "*" || key.Method == request.Method;
            return isPathMatched && isMethodMatched;
        }

        public static bool IsAjaxRequest(this HttpRequest request)
            => (request.Headers != null && request.Headers.XRequestedWith == "XMLHttpRequest")
            || (request.Headers != null && request.Headers["IsPostAjax"] == "true");
    }
}
