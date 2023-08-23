using System.Web;
using AlphaPlus.User.Models.Helpers;

namespace Alpha.Web.Helpers
{
    public class HttpContextHelper : IHttpContextHelper
    {
        public string SessionId => HttpContext.Current.Session.SessionID;

        public object GetSession(string key)
        {
            return HttpContext.Current.Session[key];
        }

        public void SetSession(string key, object value)
        {

            HttpContext.Current.Session[key] = value;
        }
    }
}