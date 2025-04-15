using AlphaPlus.Core.Models;
using Fanex.Data;

namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal class GetRewardDetailCriteria(
    int custId,
    UserLevel custLevel,
    DateTime fromDate,
    DateTime toDate,
    string programIds,
    bool? paymentStatus,
    string username,
    int pageIndex,
    int pageSize) : GetRewardCriteria(custId, fromDate, toDate, programIds, paymentStatus, username)
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRecords { get; set; }

    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_Rpt_RewardDetail_Master"
        : CustLevel.IsMasterLevel()
            ? "Age_Rpt_RewardDetail_Agent"
            : "Age_Rpt_RewardDetail_Member";
}