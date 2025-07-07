using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria;

public class CheckUsernameCommand(string username) : INonQueryCommand
{
    public string Username { get; } = username;
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public string GetSettingKey() => "API_CustomerInsMember_CashMember_CheckUsername";

    public bool IsValid() => true;
}
