using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.Core.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ProtectedModeFilterAttribute : TypeFilterAttribute
{
    public ProtectedModeFilterAttribute() : base(typeof(ProtectedModeFilter)) { }

    public class ProtectedModeFilter(IUserSession userSession) : IAuthorizationFilter
    {
        private readonly IUserSession userSession = userSession;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Result is UnauthorizedResult)
            {
                return;
            }

            if (userSession.IsProtectedMode)
            {
                var allowProtectedMode = context.ActionDescriptor.EndpointMetadata.OfType<AllowProtectedModeAttribute>().Any();

                if (!allowProtectedMode)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }
    }
}
