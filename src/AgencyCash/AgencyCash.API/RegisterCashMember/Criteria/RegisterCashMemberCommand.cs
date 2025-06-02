using Fanex.Data.Repository;

namespace AgencyCash.API.RegisterCashMember.Criteria;

public class RegisterCashMemberCommand : INonQueryCommand
{
    public int CustId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Username { get; }
    public string Username2 { get; }
    public string UserPwd { get; }
    public string MobilePhone { get; }
    public int? BankId { get; } = null; // Redundant, default null
    public string AccountNumber { get; } = null!; // Redundant, default null
    public int UplineId { get; }
    public string IP { get; }
    public byte? SettingStatus { get; } = null; // Redundant, default null
    public int ErrCode { get; set; }
    public string ErrMsg { get; set; } = string.Empty;

    public RegisterCashMemberCommand(
        int custId,
        string firstName,
        string lastName,
        string username,
        string username2,
        string userPwd,
        string mobilePhone,
        int uplineId,
        string ip)
    {
        CustId = custId;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Username2 = username2;
        UserPwd = userPwd;
        MobilePhone = mobilePhone;
        BankId = null; // Always default
        AccountNumber = null!; // Always default
        UplineId = uplineId;
        IP = ip;
        SettingStatus = null; // Always default
    }

    public RegisterCashMemberCommand() { }

    public string GetSettingKey() => "API_CustomerInsMember_CashMember";
    public bool IsValid() => !string.IsNullOrEmpty(Username);
}