using Fanex.Data.Repository;

namespace AlphaPlus.Identity.Features.Authorization.Criteria;

public class GetAccountStatusCommand(int custId, int subAccId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public int SubAccId { get; } = subAccId;

    public bool IsActive { get; set; }

    public byte Locked { get; set; }

    public override string GetSettingKey() => "Age_CustomerStatusSel";

    public override bool IsValid() => CustId > 0 && SubAccId >= 0;

}
