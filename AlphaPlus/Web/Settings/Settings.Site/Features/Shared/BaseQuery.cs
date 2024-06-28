using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Shared.Models;

namespace AlphaPlus.Settings.Site.Features.Shared
{
    public abstract class BaseQuery
    {
        protected readonly IUser viewer;

        public BaseQuery() : this(null) { }

        internal BaseQuery(IUser viewer)
        {
            this.viewer = viewer ?? new User();
            CustId = this.viewer.Id;
            CustLevelId = this.viewer.LevelId;
            CustName = this.viewer.Name;
        }

        public int CustId { get; set; }

        public byte CustLevelId { get; set; }

        public virtual UserLevel CustLevel => Enumeration.FromValue<UserLevel>(CustLevelId);

        public string CustName { get; set; }

        public int ViewerId => viewer.Id;

        public int SubAccId => viewer.SubAccId;

        public string ViewerUsername => string.IsNullOrWhiteSpace(viewer.SubAccName) ? viewer.Name : viewer.SubAccName;

        public byte ViewerLevelId => viewer.LevelId;

        public UserLevel ViewerLevel => Enumeration.FromValue<UserLevel>(ViewerLevelId);

        public string AGIP => viewer.AGIP;

        public bool CanViewReports => viewer.CanViewReports;

        public bool CanViewLog => viewer.SubPageAccess.ViewLog;

        public bool AllowTransfer => viewer.CanTransfer;

        public bool CanCreateAccount => viewer.SubPageAccess.CreateAccount;

        public bool CanUpdateAccount => viewer.CanUpdateAccount;

        public bool AllowedTotalBalanceTransfer => viewer.IsAllowedTotalBalanceTransfer;

        public bool IsInternal => viewer.IsInternal;

        public bool IsAdminCanViewVNIP => viewer.IsAdminCanViewVNIP;

        public string Language => viewer.Language;

        public string LangKey => viewer.LangKey;

        public bool IsDarkMode => !string.IsNullOrWhiteSpace(viewer.DarkModeName);

        public string IP => viewer.IP;

        public bool IsSubAcc => viewer.IsSubAcc;

        public string Currency => viewer.Currency;

        public int CurrencyId => viewer.CurrencyId;

        public string Site => viewer.Site;

        public virtual bool IsValid() => ViewerId > 0;
    }
}