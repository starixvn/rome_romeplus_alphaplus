using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria;

internal class GetCustInfoCriteria(string username, int uplineId) : CriteriaBase
{
    public string Username { get; } = username;

    public int UplineId { get; } = uplineId;

    public override string GetSettingKey() => "Age_Rpt_GetCustomerInfo";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Username) && UplineId > 0;
}