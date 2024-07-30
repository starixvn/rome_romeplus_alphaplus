using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferCondition;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferCondition;

public class TransferConditionViewModel(TransferConditionQuery message, string transferCondition)
{
    public int CustId { get; } = message.CustId;

    public bool Daily { get; } = "00000000" == transferCondition;

    public IEnumerable<SelectListItem> Weekly
    {
        get
        {
            if (string.IsNullOrWhiteSpace(transferCondition) || transferCondition.Length != 8)
            {
                yield break;
            }

            var dayOfWeekArray = transferCondition.ToCharArray();
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
