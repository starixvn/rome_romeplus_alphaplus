using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments;

public class GetMobilePrefixesCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_CashService_MobilePrefixes_Sel";

    public override bool IsValid() => true;
} 