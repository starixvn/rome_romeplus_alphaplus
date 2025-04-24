using AlphaPlus.Core.Models;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class BaseHandbookRequest
    {
        public BaseHandbookRequest() : this(null) { }

        public BaseHandbookRequest(IUserSession userSession)
        {
            userSession = userSession ?? UserSession.Current;
            
            CustId = userSession.CustId;
            OnUserId = userSession.OnUserId;
            UserId = userSession.UserId;
            SubAccId = userSession.SubAccId;
            
            LanguageKey = userSession.LangKey;
            Language = userSession.Language;
            RoleId = userSession.RoleId;
            SessionID = userSession.NetSessionId;
            Site = userSession.Site;
        }

        public int CustId { get; }
        public int OnUserId { get; }
        public int UserId { get; }
        public int SubAccId { get; }

        public string LanguageKey { get; }
        public byte RoleId { get; }
        public string SessionID { get; }
        public string Language { get; }

        public string Site { get; set; }
    }
}
