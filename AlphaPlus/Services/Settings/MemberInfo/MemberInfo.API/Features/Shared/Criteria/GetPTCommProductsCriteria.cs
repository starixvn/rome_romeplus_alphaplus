using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.Shared.Criteria;

public class GetPTCommProductsCriteria(int custId, byte settingStatus, byte type) : CriteriaBase
{
    public int CustId { get; } = custId;

    public byte SettingStatus { get; } = settingStatus;

    public byte Type { get; } = type;

    public override string GetSettingKey() => "Acc_GetProductList";

    public override bool IsValid() => CustId > 0;
}