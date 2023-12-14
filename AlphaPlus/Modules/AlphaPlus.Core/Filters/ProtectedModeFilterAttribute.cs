using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.Core.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ProtectedModeFilterAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Result is UnauthorizedResult)
        {
            return;
        }

        if (UserSession.Current.IsProtectedMode)
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
