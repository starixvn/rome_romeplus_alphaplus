namespace Alpha.MemberInfo.Site.Features.CustomerList.Requests
{
    using Alpha.MemberInfo.Site.Features.Shared.Requests;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class CustomerStatusQueryBase : BaseRequest
    {
        private readonly IUser viewer;

        public CustomerStatusQueryBase(IUser viewer)
            : base(viewer)
        {
            this.viewer = viewer;
        }

        public virtual int SearchAccountId { get; set; }

        public virtual string SearchAccountName { get; set; }

        public byte SearchAccountRoleId { get; set; }

        public int UplineId { get; set; }

        public string UplineName { get; set; }

        public byte UplineRoleId { get; set; }

        public int ActorId => viewer.Id;

        public byte ActorRoleId => viewer.LevelId;

        public string ActorName => viewer.Name;

        public int SubAccountId => viewer.SubAccId;

        public bool GetDisabledProduct(int productId) => viewer.GetDisabledProduct(productId);

        public bool GetHideProduct(int productId) => viewer.GetHideProduct(productId);

        public bool GetSyncedProduct(int productId) => viewer.GetSyncedProduct(productId);

        public bool IsDisabledESports => viewer.IsDisabledESports;

        public override bool IsValid() => IsValidSearchAccount && IsValidUpline;

        internal bool IsValidSearchAccount =>
            SearchAccountId > 0 &&
            !SearchAccountId.Equals(viewer.Id) &&
            IsValidName(SearchAccountName) &&
            IsValidSearchAccountRoleId(SearchAccountRoleId);

        internal bool IsValidUpline =>
            IsValidName(UplineName) &&
            UplineRoleId > SearchAccountRoleId;

        internal bool IsValidSearchAccountRoleId(byte searchAccountRoleId)
           => searchAccountRoleId < viewer.LevelId;

        internal bool IsValidName(string name) => !string.IsNullOrEmpty(name);

        public bool IsOfferProtectedMode => viewer.IsOfferProtectedMode;

        public string DarkModeName => viewer.DarkModeName;

        public PageAccess SubPageAccess => viewer.SubPageAccess;

        public bool SupportAutoTransfer { get; set; }
        public bool AutoTransferAffect { get; set; }
    }
}