namespace AlphaPlus.Settings.MemberInfo.Domain.Params;

using System.Diagnostics.CodeAnalysis;

[SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Constructor by design, no refactor planned")]
public class WinLossLimitParam(
    int productId,
    int custId,
    int betType,
    int sportType,
    string username,
    string playerId,
    int masterId,
    int agentId,
    int pageSize,
    int pageIndex,
    bool isDirectMember,
    int accountType,
    int siteDbGroupId)
{
    private const int SabaclubProductId = 42;

    public int ProductId { get; set; } = productId;

    public bool IsSabaclub => ProductId == SabaclubProductId;

    public int CustId { get; set; } = custId;

    public int BetType { get; set; } = betType;

    public int SportType { get; set; } = sportType;

    public int AccountType { get; set; } = accountType;

    public string Username { get; set; } = username;

    public string PlayerId { get; set; } = playerId;

    public int MasterId { get; set; } = masterId;

    public int AgentId { get; set; } = agentId;

    public int PageSize { get; set; } = pageSize;

    public int PageIndex { get; set; } = pageIndex;

    public bool IsDirectMember { get; } = isDirectMember;

    public int SiteDbGroupId { get; set; } = siteDbGroupId;
}