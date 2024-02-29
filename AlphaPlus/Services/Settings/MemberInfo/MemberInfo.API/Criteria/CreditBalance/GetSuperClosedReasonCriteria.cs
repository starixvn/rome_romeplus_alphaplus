namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance
{
    public class GetSuperClosedReasonCriteria : GetCreditBalanceCriteriaBase
    {
        public GetSuperClosedReasonCriteria(
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

        public override string GetSettingKey()
            => "Age_CustomerSelCreditBalClosedReason_SAg";
    }
}
