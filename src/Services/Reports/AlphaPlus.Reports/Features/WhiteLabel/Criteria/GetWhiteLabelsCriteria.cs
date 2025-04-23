using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.WhiteLabel;

internal class GetWhiteLabelsCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Age_WhiteLabelListSel";

    public override bool IsValid() => CustId > 0;
}