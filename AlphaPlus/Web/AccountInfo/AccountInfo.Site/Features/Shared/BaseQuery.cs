using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.Shared
{
    public abstract class BaseQuery
    {
        protected readonly IUserSession userSession;

        public BaseQuery() : this(new UserSessionExt()) { }

        protected BaseQuery(IUserSession userSession)
        {
            this.userSession = userSession;
            CustId = userSession.CustId;
        }

        public int CustId { get; set; }

        public int ViewerId => userSession.CustId;

        public byte ViewerLevelId => userSession.RoleId;

        public int SubAccountId => userSession.SubAccId;

        public UserLevel ViewerLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public string ViewerName => userSession.UserName;

        public bool IsInternal => userSession.IsInternal;

        public bool AllowedTotalBalanceTransfer => userSession.IsAllowedTotalBalanceTransfer;

        public bool AllowTransfer => userSession.SubPageAccess.Transfer;

        public bool ViewMemberInfo => userSession.SubPageAccess.ViewMemberInfo;

        public bool CanViewReports => userSession.SubPageAccess.ViewReports;

        public bool CanViewLog => userSession.SubPageAccess.ViewLog;

        public bool CanCreateAccount => userSession.SubPageAccess.CreateAccount;

        public bool CanUpdateAccount
            => !userSession.IsSubAcc || (userSession.IsSubAcc && userSession.SubPageAccess.UpdateAccount);

        public string AGIP => userSession.AGIP;

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public string LangKey => userSession.LangKey;

        public string Language => userSession.Language;

        public string IP => userSession.UserLoginIp;

        public int ServerId => userSession.ServerId;

        public virtual bool IsValid() => ViewerId > 0;
    }
}