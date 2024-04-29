using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.Shared.Requests
{
    public abstract class BaseQuery
    {
        protected readonly IUserSession userSession;

        public BaseQuery() : this(null) { }

        internal BaseQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int ActorId => userSession.CustId;

        public string ActorName => userSession.UserName;

        public byte ActorRoleId => userSession.RoleId;

        public UserLevel ActorLevel => Enumeration.FromValue<UserLevel>(userSession.RoleId);

        public string AGIP => userSession.AGIP;

        public bool CanViewReports => userSession.SubPageAccess.ViewReports;

        public bool CanViewLog => userSession.SubPageAccess.ViewLog;

        public bool AllowTransfer => userSession.SubPageAccess.Transfer;

        public bool CanUpdateAccount
           => !userSession.IsSubAcc || (userSession.IsSubAcc && userSession.SubPageAccess.UpdateAccount);

        public bool AllowedTotalBalanceTransfer => userSession.IsAllowedTotalBalanceTransfer;

        public bool IsInternal => userSession.IsInternal;

        public bool IsAdminCanViewVNIP => userSession.IsAdminCanViewVNIP;

        public virtual bool IsValid() => ActorId > 0;
    }
}