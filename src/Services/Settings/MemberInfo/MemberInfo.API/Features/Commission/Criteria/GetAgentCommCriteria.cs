using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Commission;

internal class GetAgentCommCriteria(int productId, int agentId, byte settingStatus, bool isDirectDownline) : CriteriaBase
{
    public int ProductId { get; } = productId;

    public int Recommend { get; } = agentId;

    public byte SettingStatus { get; } = settingStatus;

    public bool IsDirectDownline { get; } = isDirectDownline;

    public override string GetSettingKey() => "Age_CustomerSelCommByProductID_Ag";

    public override bool IsValid() => Recommend > 0 && ProductId > 0;
}