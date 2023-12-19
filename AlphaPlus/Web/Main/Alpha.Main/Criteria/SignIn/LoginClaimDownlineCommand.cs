using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.Main.Criteria.SignIn;

public class LoginClaimDownlineCommand : INonQueryCommand
{
    public int CustId { get; init; }

    [SpParam(SqlParamName = "userpwd")]
    public string Password { get; init; }

    public string UserName { get; set; } = string.Empty;

    public string Nickname { get; init; }

    public bool IsNickname { get; init; }

    public int SubAccId { get; init; }

    public string SubUsername { get; set; }

    public DateTime LoginTime { get; init; }

    public string Language { get; init; }

    public int RoleId { get; set; } = 0;

    public string LoginIp { get; init; }

    public string CountryCode { get; init; }

    public string LoginDomain { get; init; }

    public string BrowserName { get; init; }

    public string Platform { get; init; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string GetSettingKey() => "Age_LoginClaimDownline";

    public bool IsValid() => CustId > 0;

}
