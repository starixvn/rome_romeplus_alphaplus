using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth;

public class InternalLoginCriteria : NonQueryCommand
{
    [SpParam(SqlParamName = "accuserid")]
    public int AdminId { get; set; }

    public string Username { get; set; }

    public DateTime LoginTime { get; set; }

    public string Language { get; set; }

    public string LoginIp { get; set; }

    public string CountryCode { get; set; }

    public string LoginDomain { get; set; }

    public string BrowserName { get; set; }

    public string Platform { get; set; }

    public string SessionId { get; set; }

    public int CustId { get; set; }

    public byte RoleId { get; set; }

    public int On_UserId { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Age_LoginDownline_Admin";

    public override bool IsValid() => AdminId > 0 && !string.IsNullOrWhiteSpace(Username);
}