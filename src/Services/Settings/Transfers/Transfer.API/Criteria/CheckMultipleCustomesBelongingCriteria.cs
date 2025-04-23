using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria;

internal class CheckMultipleCustomesBelongingCriteria(
    int actorId,
    int subAccId,
    string custIds) : NonQueryCommand
{
    public int SessionCustId { get; } = actorId;

    public int SubAccountId { get; } = subAccId;

    public string CustId { get; } = custIds;

    public byte RoleId { get; set; }

    public bool IsActive { get; set; }

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_MultiCust";

    public override bool IsValid() => SessionCustId > 0 && !string.IsNullOrWhiteSpace(CustId);
}