namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;

using System.Diagnostics.CodeAnalysis;
using Core.Criteria;
using Core.Helpers;
using Fanex.Data;

[SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Constructor by design, no refactor planned")]
public class GetBalanceCriteria(
    int custId,
    int betType,
    string username,
    int masterId,
    int agentId,
    int pageIndex,
    int pageSize,
    bool isDirectMember,
    int accountType,
    int siteDbGroupId)
    : SiteDbBaseCommand(siteDbGroupId)
{
    public int CustId { get; set; } = custId;

    public int BetType { get; set; } = betType;

    public string Username { get; set; } = username;

    [SpParam(SqlParamName = "Mrecommend")]
    public int MasterId { get; set; } = masterId;

    [SpParam(SqlParamName = "Recommend")]
    public int AgentId { get; set; } = agentId;

    public int PageSize { get; set; } = pageSize;

    public int PageIndex { get; set; } = pageIndex;

    public bool IsDirectDownline { get; set; } = isDirectMember;

    public int AccountType { get; set; } = accountType;

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Age_CustomerSelWinlimitInfo", siteDbGroupId);

    public override bool IsValid()
        => CustId > 0;
}