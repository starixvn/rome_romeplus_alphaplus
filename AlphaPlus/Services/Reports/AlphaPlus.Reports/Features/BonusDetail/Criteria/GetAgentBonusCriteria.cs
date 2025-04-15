namespace AlphaPlus.Reports.Features.BonusDetail;

public class GetAgentBonusCriteria(
    string username,
    DateTime fromDate,
    DateTime toDate,
    byte? statusId,
    bool? isComplained,
    int pageIndex,
    int pageSize,
    int agentId) : GetBonusDetailCriteria(username, fromDate, toDate, statusId, isComplained, pageIndex, pageSize)
{
    public int AgentId { get; } = agentId;

    public override string GetSettingKey() => "Age_Rpt_BonusDetail_Agent";

    public override bool IsValid() => base.IsValid() && AgentId > 0;
}
