using Fanex.Data;

namespace AlphaPlus.Identity.API.Features.Authorization;

public class CheckBelongingMultipleCustomersCommand(
    int sessionCustId,
    int subAccId,
    string custIds) : CheckBelongingBaseCommand(sessionCustId, subAccId)
{
    [SpParam(SqlParamName = "custid")]
    public string CustIds { get; } = custIds;

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_MultiCust";
}
