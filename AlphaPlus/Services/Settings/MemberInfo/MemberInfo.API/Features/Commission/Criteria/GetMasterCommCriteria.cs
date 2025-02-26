using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Commission;

internal class GetMasterCommCriteria(int productId, int masterId, byte settingStatus) : CriteriaBase
{
    public int ProductId { get; } = productId;

    public int MRecommend { get; } = masterId;

    public byte SettingStatus { get; } = settingStatus;

    public override string GetSettingKey() => "Age_CustomerSelCommByProductID_MAg";

    public override bool IsValid() => MRecommend > 0 && ProductId > 0;
}