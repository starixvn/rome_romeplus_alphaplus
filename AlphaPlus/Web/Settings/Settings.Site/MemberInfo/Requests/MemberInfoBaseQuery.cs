namespace AlphaPlus.Settings.Site.Requests
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.Site.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class MemberInfoBaseQuery
    {
        protected readonly IUser viewer;

        protected MemberInfoBaseQuery() : this(null)
        {
        }

        protected MemberInfoBaseQuery(IUser viewer)
        {
            this.viewer = viewer ?? new User(UserSession.Current);

            // Default data, if query string had data, then get from query string.
            UserId = this.viewer.LoggedInUserId;
            CustId = this.viewer.Id;
            CustName = this.viewer.Name;
            Site = this.viewer.Site;
            CustLevelId = this.viewer.LevelId;
            SubAccId = this.viewer.SubAccId;
            SubUsername = this.viewer.SubAccName;
            BreadcrumbLevel = Enumerable.Empty<CustInfo>();
        }

        public string ActorUsername => viewer.Name;

        public int UserId { get; set; }

        public string SubUsername { get; set; }

        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string CustName { get; set; }

        public string Site { get; set; }

        public byte CustLevelId { get; set; }

        public virtual Settings.MemberInfo.Domain.Models.UserLevel CustLevel => Settings.MemberInfo.Domain.Enumerations.Enumeration.FromValue<Settings.MemberInfo.Domain.Models.UserLevel>(CustLevelId);

        public int ViewerId => viewer.Id;

        public int ViewerSubAccId => viewer.SubAccId;

        public string ViewerUsername => string.IsNullOrWhiteSpace(viewer.SubAccName) ? viewer.Name : viewer.SubAccName;

        public bool CanUpdateAccount => viewer.CanUpdateAccount;

        public byte ViewerLevelId => viewer.LevelId;

        public virtual Settings.MemberInfo.Domain.Models.UserLevel ActorLevel => Settings.MemberInfo.Domain.Enumerations.Enumeration.FromValue<Settings.MemberInfo.Domain.Models.UserLevel>(ViewerLevelId);

        public string Language => viewer.Language;

        public string LanguageKey => viewer.LanguageKey;

        public string IP => viewer.IP;

        public bool IsInternal => viewer.IsInternal;

        public bool IsAllowedIndirectLogin => viewer.IsAllowedIndirectLogin;

        public bool IsAdminCanViewVNIP => viewer.IsAdminCanViewVNIP;

        public IEnumerable<CustInfo> BreadcrumbLevel { get; set; }

        public bool IsDarkMode => !string.IsNullOrWhiteSpace(viewer.DarkModeName);

        public string AGIP => viewer.AGIP;

        public virtual bool IsValid()
            => CustId > 0
            && !string.IsNullOrWhiteSpace(CustName)
            && ActorLevel.IsEqualOrUplineOf(CustLevel)
            && !string.IsNullOrWhiteSpace(Language);
    }
}