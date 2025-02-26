using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Params
{
    public class SelectCloseInfoCriteria : CriteriaBase
    {
        public SelectCloseInfoCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override bool IsValid() => CustId > 0;

        public override string GetSettingKey() => "Acc_Customer_CloseInfoSel";
    }
}