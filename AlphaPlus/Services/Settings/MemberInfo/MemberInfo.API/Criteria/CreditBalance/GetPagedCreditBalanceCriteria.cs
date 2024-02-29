namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance
{
    public abstract class GetPagedCreditBalanceCriteria : GetCreditBalanceCriteriaBase
    {
        protected GetPagedCreditBalanceCriteria(
            int custId,
            string userName,
            bool? enabled,
            bool? closed,
            bool? suspended,
            int pageIndex,
            int pageSize)
            : base(custId, userName, enabled, closed, suspended, pageIndex, pageSize)
        {
        }

        public int TotalItems { get; set; }
    }
}
