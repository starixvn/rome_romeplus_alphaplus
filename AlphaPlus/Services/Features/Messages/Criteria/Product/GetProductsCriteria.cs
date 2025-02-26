using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria.Product
{
    public class GetProductsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_Rpt_GetProductList";

        public override bool IsValid() => true;
    }
}
