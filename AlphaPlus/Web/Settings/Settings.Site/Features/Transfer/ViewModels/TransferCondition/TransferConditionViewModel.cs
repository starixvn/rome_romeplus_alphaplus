using AlphaPlus.Shared.AppResources.Settings.Transfer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferCondition;

public class TransferConditionViewModel(
    int custId,
    string transferOption,
    string warningMessage,
    bool isEnableAutoTransfer)
{
    public int CustId { get; } = custId;

    public string TransferOption { get; } = transferOption;

    public string WarningMessage { get; } = warningMessage;

    public bool IsEnableAutoTransfer { get; } = isEnableAutoTransfer;

    public bool Daily { get; } = "00000000" == transferOption;

    public IEnumerable<SelectListItem> Weekly
    {
        get
        {
            if (string.IsNullOrWhiteSpace(TransferOption) || TransferOption.Length != 8)
            {
                yield break;
            }

            var dayOfWeekArray = TransferOption.ToCharArray();
            var dayOfWeekArrayLength = dayOfWeekArray.Length - 1;

            for (var i = 1; i < dayOfWeekArrayLength; i++)
            {
                yield return new SelectListItem(GetDayOfWeekName((DayOfWeek)i), i.ToString(), dayOfWeekArray[i] == '1');
            }

            yield return new SelectListItem(GetDayOfWeekName(DayOfWeek.Sunday), "0", dayOfWeekArray[0] == '1');
        }
    }

    private static string GetDayOfWeekName(DayOfWeek dayOfWeek)
        => dayOfWeek switch
        {
            DayOfWeek.Sunday => Resources.Sun,
            DayOfWeek.Monday => Resources.Mon,
            DayOfWeek.Tuesday => Resources.Tue,
            DayOfWeek.Wednesday => Resources.Wed,
            DayOfWeek.Thursday => Resources.Thurs,
            DayOfWeek.Friday => Resources.Fri,
            DayOfWeek.Saturday => Resources.Sat,
            _ => string.Empty
        };
}
