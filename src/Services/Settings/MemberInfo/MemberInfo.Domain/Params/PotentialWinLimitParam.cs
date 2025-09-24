namespace AlphaPlus.Settings.MemberInfo.Domain.Params;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Constructor by design, no refactor planned")]
public class PotentialWinLimitParam(
    int custId,
    byte roleId,
    string username,
    string status,
    byte problemAccount,
    int pageSize,
    int pageIndex,
    int siteDbGroupId,
    int betType = 0,
    int sportType = 0,
    int accountType = 0,
    int settingStatus = 0)
{
    public int CustId { get; set; } = custId;

    public byte RoleId { get; set; } = roleId;

    public int BetType { get; set; } = betType;

    public int SportType { get; set; } = sportType;

    public string Username { get; set; } = username;

    public string Status { get; set; } = string.IsNullOrEmpty(status) ? "All" : status;

    public byte ProblemAccount { get; set; } = problemAccount;

    public int PageSize { get; set; } = pageSize;

    public int PageIndex { get; set; } = pageIndex;

    public int SiteDbGroupId { get; } = siteDbGroupId;

    public int AccountType { get; set; } = accountType;

    public int SettingStatus { get; set; } = settingStatus;
}