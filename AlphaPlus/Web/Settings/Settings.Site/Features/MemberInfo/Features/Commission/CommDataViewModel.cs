using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommDataViewModel(CommDataQuery message, CommissionReport reportData)
{
    public UserLevel ActorLevel { get; } = message.ActorLevel;

    public bool IsShowGroup { get; } = message.ProductId == ProductIdConstants.Sportsbook;

    public bool IsShowDoubleComm { get; } = message.IsShowDoubleComm;

    public CommissionReport ReportData { get; } = reportData;
}
