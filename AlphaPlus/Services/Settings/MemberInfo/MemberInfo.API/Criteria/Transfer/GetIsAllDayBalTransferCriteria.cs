using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.Transfer
{
    public class GetIsAllDayBalTransferCriteria : CriteriaBase
    {
        public GetIsAllDayBalTransferCriteria(int custId)
            => CustId = custId;

        public int CustId { get; }

        public override string GetSettingKey()
            => "Age_GetIsAllDayBalTransfer";

        public override bool IsValid()
            => CustId > 0;
    }
}
