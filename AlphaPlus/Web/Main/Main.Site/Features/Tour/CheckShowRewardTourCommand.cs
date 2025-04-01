using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Tour;

public class CheckShowRewardTourCommand(int custId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public bool IsShow { get; set; }

    public override string GetSettingKey() => "Age_ValidateShowTourGuide";

    public override bool IsValid() => CustId > 0;
}