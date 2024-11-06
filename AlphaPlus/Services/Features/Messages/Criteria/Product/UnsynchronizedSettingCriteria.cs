using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria.Product
{
    public class UnsynchronizedSettingCriteria : CriteriaBase
    {
        public UnsynchronizedSettingCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Acc_CustomerSelUnSyncProducts";

        public override bool IsValid() => CustId > 0;
    }
}
