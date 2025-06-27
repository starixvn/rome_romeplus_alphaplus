using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

public class GetCountryCodesCriteria(string siteName) : CriteriaBase
{
    [SpParam(SqlParamName = "Site")]
    public string SiteName { get; } = siteName;

    public override string GetSettingKey() => "Age_CashService_CountryCodeSel";

    public override bool IsValid() => true;
} 