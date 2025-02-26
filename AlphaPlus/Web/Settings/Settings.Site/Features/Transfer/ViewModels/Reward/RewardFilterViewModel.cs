using AlphaPlus.Core.Utils;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;
using AlphaPlus.Settings.Transfer.API.Models.Reward;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Core.Extensions;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.Components;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Reward;

public class RewardFilterViewModel
{
    public RewardFilterViewModel(
        RewardBaseQuery message,
        IEnumerable<RewardProgram> programs,
        string excelAction,
        bool showColumns = false)
    {
        DateRangePicker = new DateRangePickerJQueryViewModel(
            Resources.DateRange,
            message.Language,
            message.FromDate,
            message.ToDate,
            message.MinDate,
            SystemTime.Today.AddDays(1));
        Programs = GetPrograms(programs, message.ProgramIds);
        PaymentStatus = GetPaymentStatus(message.PaymentStatus);
        Username = message.Username;
        ExcelAction = excelAction;
        ShowColumns = showColumns;
    }

    public DateRangePickerJQueryViewModel DateRangePicker { get; }

    public IEnumerable<SelectListItem> Programs { get; }

    public IEnumerable<SelectListItem> PaymentStatus { get; }

    public string Username { get; }

    public bool ShowColumns { get; }

    public string ExcelAction { get; }

    private static IEnumerable<SelectListItem> GetPrograms(IEnumerable<RewardProgram> programs, string selectedProgramIds)
    {
        var programIds = selectedProgramIds.ToListInt().ToList();

        return programs.Select(p => new SelectListItem(
            p.Name,
            p.ResourceId.ToString(),
            programIds.Contains(p.ResourceId)));
    }

    private static IEnumerable<SelectListItem> GetPaymentStatus(bool? selectedPaymentStatus)
        => [
            new SelectListItem(Resources.All, "0", selectedPaymentStatus == null),
            new SelectListItem(Resources.Paid, true.ToString(), selectedPaymentStatus == true),
            new SelectListItem(Resources.Unpaid, false.ToString(), selectedPaymentStatus == false)
        ];
}