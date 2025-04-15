using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Commission;

internal class GetMasterCommCriteria(int productId, int masterId, byte settingStatus, bool isDirectDownline) : CriteriaBase
{
    public int ProductId { get; } = productId;

    public int MRecommend { get; } = masterId;

    public byte SettingStatus { get; } = settingStatus;

    public bool IsDirectDownline { get; } = isDirectDownline;

    public override string GetSettingKey() => "Age_CustomerSelCommByProductID_MAg";

    public override bool IsValid() => MRecommend > 0 && ProductId > 0;
}