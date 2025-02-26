using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Criteria
{
    public class SelectSuspendInfoCriteria : CriteriaBase
    {
        public SelectSuspendInfoCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Acc_Customer_SuspendInfoSel";

        public override bool IsValid() => CustId > 0;
    }
}