using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public class BetChoiceUplinesCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Age_Customer_Upline_Sel";

    public override bool IsValid() => CustId > 0;
}