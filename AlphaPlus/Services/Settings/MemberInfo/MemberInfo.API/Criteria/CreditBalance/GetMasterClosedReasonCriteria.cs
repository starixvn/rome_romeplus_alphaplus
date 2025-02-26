namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance
{
    public class GetMasterClosedReasonCriteria : GetCreditBalanceCriteriaBase
    {
        public GetMasterClosedReasonCriteria(
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
            => "Age_CustomerSelCreditBalClosedReason_MAg";
    }
}
