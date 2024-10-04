using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Core.Criteria;

public class CheckMultipleCustomerBelongingCommand(int sessionCustId, int subAccId, string custIds) : NonQueryCommand
{
    public int SessionCustId { get; } = sessionCustId;

    public int SubAccountId { get; } = subAccId;

    [SpParam(SqlParamName = "custid")]
    public string CustIds { get; } = custIds;

    public byte RoleId { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_MultiCust";

    public override bool IsValid() => SessionCustId > 0 && SubAccountId >= 0;

}
