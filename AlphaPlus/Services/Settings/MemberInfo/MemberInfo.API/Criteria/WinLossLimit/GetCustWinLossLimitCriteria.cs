using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit;

public class GetCustWinLossLimitCriteria : CriteriaBase
{
    public GetCustWinLossLimitCriteria(
        int custId,
        int productId,
        int betType,
        int sportType,
        string username,
        int masterId,
        int agentId,
        int pageIndex,
        int pageSize,
        int? playerId,
        bool isDirectMember)
    {
        PlayerId = playerId;

        CustId = custId;
        ProductId = productId;
        BetType = betType;
        SportType = sportType;
        Username = username;
        MasterId = masterId;
        AgentId = agentId;
        PageIndex = pageIndex;
        PageSize = pageSize;
        IsDirectDownline = isDirectMember;
    }

    public int ProductId { get; set; }

    public int CustId { get; set; }

    public int BetType { get; set; }

    public int SportType { get; set; }

    public string Username { get; set; }

    [SpParam(SqlParamName = "Mrecommend")]
    public int MasterId { get; set; }

    [SpParam(SqlParamName = "Recommend")]
    public int AgentId { get; set; }

    [SpParam(SqlParamName = "PlayerId_IBC")]
    public int? PlayerId { get; set; }

    public int PageSize { get; set; }

    public int PageIndex { get; set; }

    public int TotalRows { get; set; }

    public byte SettingStatus => AppSettings.SettingStatus;

    public bool IsDirectDownline { get; set; }

    public override string GetSettingKey() => "Age_CustomerSelCustWinlimit";

    public override bool IsValid() => CustId > 0;
}