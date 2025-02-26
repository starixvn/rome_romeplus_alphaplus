using System.ComponentModel;

namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public enum MemberWinLossOperators : byte
{
    [Description(">=")]
    GreatorOrEqual = 0,

    [Description("=")]
    Equal = 1,

    [Description("<=")]
    LessOrEqual = 2
}