using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Models.Reward;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Reward;

public class RewardDetailDataViewModel(UserLevel actorLevel, RewardDetail reportData)
{
    public UserLevel ActorLevel { get; } = actorLevel;

    public RewardDetail ReportData { get; } = reportData;
}