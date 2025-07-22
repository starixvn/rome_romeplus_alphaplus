using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria;

public class ValidateSettingCommand(string username, string domain, int agentId) : INonQueryCommand
{
    public string Username { get; } = username;
    public string Domain { get; } = domain;
    public int AgentId { get; } = agentId;

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string AgentName { get; set; }

    public string GetSettingKey() => "API_CustomerInsMember_CashMember_Validate";
    public bool IsValid() => !string.IsNullOrWhiteSpace(Username) && AgentId > 0;
}
