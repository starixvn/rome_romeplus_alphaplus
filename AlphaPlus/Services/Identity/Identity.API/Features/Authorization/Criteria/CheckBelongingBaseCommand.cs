using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Authorization;

public abstract class CheckBelongingBaseCommand(int sessionCustId, int subAccId) : NonQueryCommand
{
    public int SessionCustId { get; } = sessionCustId;

    public int SubAccountId { get; } = subAccId;

    public byte RoleId { get; set; }

    public bool IsActive { get; set; } = true;

    public override bool IsValid() => SessionCustId > 0 && SubAccountId >= 0;
}
