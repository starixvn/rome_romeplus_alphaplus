using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments;

public class GetCountryCodesCriteria(int custId) : CriteriaBase
{
    public int CustId { get; set; } = custId;

    public override string GetSettingKey() => "Age_CashService_CountryPrefix_Sel";

    public override bool IsValid() => true;
} 