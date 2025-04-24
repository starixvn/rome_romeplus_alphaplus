using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Authorization;

public class CheckBelongingByUsernameCommand(int uplineId, int subAccountId, string username) : NonQueryCommand
{
    public int UplineId { get; } = uplineId;

    public int SubAccountId { get; } = subAccountId;

    public string Username { get; } = username;

    public bool IsActive { get; set; }

    public int CustId { get; set; }

    public bool IsSubAcc { get; set; }

    public int RoleId { get; set; }

    public int AgentId { get; set; }

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_ByUsername";

    public override bool IsValid() => UplineId > 0 && !string.IsNullOrWhiteSpace(Username);
}