using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class SelectCustomerListCriteriaBase : CriteriaBase
    {
        public SelectCustomerListCriteriaBase(
            string username,
            bool? enabled,
            bool? closed,
            bool? suspended,
            bool locked,
            byte doubleCommLosing,
            int autoTransfer,
            int pageSize,
            int pageIndex)
        {
            Username = username;
            Enabled = enabled;
            Closed = closed;
            Suspended = suspended;
            Locked = locked;
            DoubleCommLosing = doubleCommLosing;
            AutoTransfer = autoTransfer;

            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public string Username { get; }

        public bool? Enabled { get; }

        public bool? Closed { get; }

        public bool? Suspended { get; }

        public bool Locked { get; }

        public byte DoubleCommLosing { get; }

        public int AutoTransfer { get; set; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public override string GetSettingKey() => string.Empty;

        public override bool IsValid() => true;
    }
}