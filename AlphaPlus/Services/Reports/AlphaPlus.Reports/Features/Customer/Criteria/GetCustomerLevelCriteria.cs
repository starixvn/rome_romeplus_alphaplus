using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Customer;

public class GetCustomerLevelCriteria(int uplineId, string username) : CriteriaBase
{
    public int UplineId { get; } = uplineId;

    public string Username { get; } = username;

    public override string GetSettingKey() => "Age_Rpt_GetCustomerInfo";

    public override bool IsValid() => UplineId > 0 && !string.IsNullOrWhiteSpace(Username);
}