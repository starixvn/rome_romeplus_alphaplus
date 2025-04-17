using Fanex.Data.Repository;

namespace AlphaPlus.Identity.Features.Authorization.Criteria;

public class CheckBelongingCommand(int sessionCustId, int subAccId, long transId, int? custId = null) : NonQueryCommand
{
    public int SessionCustId { get; } = sessionCustId;

    public int SubAccountId { get; } = subAccId;

    public long TransId { get; } = transId;

    public int? CustId { get; } = custId;

    public byte RoleId { get; set; }

    public bool IsActive { get; set; } = true;

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging";

    public override bool IsValid() => SessionCustId > 0 && SubAccountId >= 0;
}
