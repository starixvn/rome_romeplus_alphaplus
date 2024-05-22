using AlphaPlus.Settings.Site.Features.Shared.Requests;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class CustomerStatusQueryBase : BaseQuery
    {
        public virtual int SearchAccountId { get; set; }

        public virtual string SearchAccountName { get; set; }

        public byte SearchAccountRoleId { get; set; }

        public int UplineId { get; set; }

        public string UplineName { get; set; }

        public byte UplineRoleId { get; set; }

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
    }
}