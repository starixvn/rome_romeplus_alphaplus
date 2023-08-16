using System.Web;
using AlphaPlus.User.Models.Helpers;

namespace AlphaPlus.Web.Helpers
{
    public class HttpContextHelper : IHttpContextHelper
    {
        public string GetSession(string key)
        {
            return IsRequestInitialized() ? HttpContext.Current.Session[key]?.ToString() : string.Empty;
        }

        public void SetSession(string key, string value)
        {
            if (IsRequestInitialized())
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        private bool IsRequestInitialized()
        {
            return HttpContext.Current != null && HttpContext.Current.Session != null;
        }
    }
}