using Microsoft.AspNetCore.Mvc.Filters;

namespace AlphaPlus.Core.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AllowProtectedModeAttribute : ActionFilterAttribute
{
}
