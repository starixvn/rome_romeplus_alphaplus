using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class SelectProductStatusCriteria(int productId, int custId) : CriteriaBase
    {
        public int ProductId { get; } = productId;

        public int CustId { get; } = custId;

        public override string GetSettingKey() => "Acc_AdminSelStatusByProductId";

        public override bool IsValid() => ProductId > 0 && CustId > 0;
    }
}