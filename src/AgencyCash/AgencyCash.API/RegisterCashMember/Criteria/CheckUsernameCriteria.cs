using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria;

public class CheckUsernameCommand : INonQueryCommand
{
    public string Username { get; }
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public CheckUsernameCommand(string username)
    {
        Username = username;
    }

    public string GetSettingKey() => "API_CustomerInsMember_CashMember_CheckUsername";

    public bool IsValid() => true;
}
