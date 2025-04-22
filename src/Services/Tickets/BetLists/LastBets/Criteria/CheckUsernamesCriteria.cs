using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public class CheckUsernamesCriteria(int agentId, string usernames, byte roleId) : CriteriaBase
{
    public int AgentId { get; } = agentId;

    public string Usernames { get; } = usernames;

    public byte RoleId { get; } = roleId;

    public override string GetSettingKey() => "Age_CustLastBetFilter_Check";

    public override bool IsValid() => AgentId > 0 && !string.IsNullOrWhiteSpace(Usernames);
}