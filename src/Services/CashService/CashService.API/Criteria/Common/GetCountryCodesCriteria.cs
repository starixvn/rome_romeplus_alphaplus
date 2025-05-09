using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Common;

public class GetCountryCodesCriteria(string siteName) : CriteriaBase
{
    [SpParam(SqlParamName = "Site")]
    public string SiteName { get; set; } = siteName;

    public override string GetSettingKey() => "Age_CashService_CountryCodeSel";

    public override bool IsValid() => true;
} 