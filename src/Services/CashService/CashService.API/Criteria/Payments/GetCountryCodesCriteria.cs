using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments;

public class GetCountryCodesCriteria : CriteriaBase
{
    public GetCountryCodesCriteria(int custId)
    {
        CustId = custId;
    }

    public int CustId { get; set; }

    public override string GetSettingKey() => "Age_CashService_CountryPrefix_Sel";

    public override bool IsValid() => true;
} 