using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria;

public class CheckUsernameCommand : INonQueryCommand
{
    public string Username { get; }

    public string ServiceType { get; set; }

    public CheckUsernameCommand(string username)
    {
        Username = username;
    }

    public string GetSettingKey() => "CheckUsername";

    public bool IsValid() => true;
}
