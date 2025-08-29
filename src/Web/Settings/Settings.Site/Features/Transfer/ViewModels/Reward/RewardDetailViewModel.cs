using AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;
using AlphaPlus.Settings.Transfer.API.Models.Reward;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Reward;

public class RewardDetailViewModel(
    RewardBaseQuery message,
    IEnumerable<RewardProgram> programs,
    RewardDetail reportData) : RewardDetailDataViewModel(message.ActorLevel, reportData)
{
    public RewardFilterViewModel Filter { get; } = new RewardFilterViewModel(
        message,
        programs,
        excelAction: "DetailExcel",
        showColumns: true,
        feedbackFunctionId: 300);

    public string VisibleColumnKey { get; } = $"RewardDetail_{message.ViewerId}_{message.SubAccId}:Columns";
}
