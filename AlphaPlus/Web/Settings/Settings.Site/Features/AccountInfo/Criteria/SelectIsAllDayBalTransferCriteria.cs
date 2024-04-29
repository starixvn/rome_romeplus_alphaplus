using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class SelectIsAllDayBalTransferCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public override string GetSettingKey()
            => "Age_GetIsAllDayBalTransfer";

        public override bool IsValid()
            => CustId > 0;
    }
}