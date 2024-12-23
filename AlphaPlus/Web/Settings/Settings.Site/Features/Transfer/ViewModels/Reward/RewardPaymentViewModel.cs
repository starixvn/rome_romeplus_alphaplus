using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;
using AlphaPlus.Settings.Transfer.API.Models.Reward;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Reward;

public class RewardPaymentViewModel(
    RewardBaseQuery message,
    IEnumerable<RewardProgram> programs,
    RewardPayment reportData)
{
    public UserLevel ActorLevel { get; } = message.ActorLevel;

    public RewardFilterViewModel Filter { get; } = new RewardFilterViewModel(message, programs, excelAction: "PaymentExcel");

    public RewardPayment ReportData { get; } = reportData;
}