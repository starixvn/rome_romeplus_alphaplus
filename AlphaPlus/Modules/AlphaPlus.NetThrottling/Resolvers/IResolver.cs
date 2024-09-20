using Microsoft.AspNetCore.Http;

namespace AlphaPlus.NetThrottling.Resolvers
{
    public interface IResolver
    {
        Task Block();

        Task Block(HttpContext context);
    }
}
