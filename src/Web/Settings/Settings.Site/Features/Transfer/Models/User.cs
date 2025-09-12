using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Models;

namespace AlphaPlus.Settings.Site.Features.Transfer.Models
{
    public class User(IUserSession userSession) : IUser
    {
        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Language => userSession.Language;

        public byte LevelId => userSession.RoleId;

        public string LanguageKey => userSession.LangKey;

        public int SubAccId => userSession.SubAccId;

        public string SubAccName => userSession.SubUserName;

        public string IP => userSession.UserLoginIp;

        public bool AllowTotalBalanceTransfer => userSession.IsAllowedTotalBalanceTransfer;

        public int UserId => userSession.UserId;

        public bool IsInternal => userSession.IsInternal;

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>("CustomerDateLimit");

        public string Site => userSession.Site;

        public string Currency => userSession.Currency;

        public bool IsSubAcc => userSession.IsSubAcc;
        public PageAccess SubPageAccess => userSession.SubPageAccess;

        public string AGIP => userSession.AGIP;
        public int SiteDbGroupId => userSession.SiteDBGroupId;
    }
}