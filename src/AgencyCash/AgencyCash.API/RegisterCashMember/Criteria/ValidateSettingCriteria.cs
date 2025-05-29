using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria;

public class ValidateSettingCommand : INonQueryCommand
{
    public string Username { get; }
    public string Domain { get; }
    public int AgentId { get; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public string AgentName { get; set; }

    public ValidateSettingCommand(string username, string domain, int agentId)
    {
        Username = username;
        Domain = domain;
        AgentId = agentId;
    }

    public string GetSettingKey() => "API_CustomerInsMember_CashMember_Validate";
    public bool IsValid() => !string.IsNullOrWhiteSpace(Username) && AgentId > 0;
}
