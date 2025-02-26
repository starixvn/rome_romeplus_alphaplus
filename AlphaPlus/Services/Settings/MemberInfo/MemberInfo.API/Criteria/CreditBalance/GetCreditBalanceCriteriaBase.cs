using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance
{
    public abstract class GetCreditBalanceCriteriaBase : CriteriaBase
    {
        protected GetCreditBalanceCriteriaBase(
            int custId,
            string userName,
            bool? enabled,
            bool? closed,
            bool? suspended,
            int pageIndex,
            int pageSize)
        {
            CustId = custId;
            UserName = userName ?? string.Empty;
            Enabled = enabled;
            Closed = closed;
            Suspended = suspended;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int CustId { get; }

        public string UserName { get; }

        public bool? Enabled { get; }

        public bool? Closed { get; }

        public bool? Suspended { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public override bool IsValid()
            => CustId > 0;
    }
}
