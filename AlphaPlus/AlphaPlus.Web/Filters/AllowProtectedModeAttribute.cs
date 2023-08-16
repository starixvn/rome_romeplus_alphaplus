namespace AlphaPlus.Web.Filters
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowProtectedModeAttribute : AuthorizeAttribute
    {
    }
}
