using AlphaPlus.Shared.AppResources.Betlist.Services;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Utils;
using Fanex.BetList.Core.Utils.StringExt;
using Fanex.BetList.Core.Utils.TicketExt;

namespace AlphaPlus.BetLists.Shared.Models;

public class TicketDto : Ticket
{
    public string Country { get; set; }

    // Properties for Last Bets
    public string MatchStatus { get; set; }

    public bool IsInWatchlist { get; set; }

    public bool IsHighlight { get; set; }

    public string BetType { get; set; }

    // End of Properties for Last Bets

    public int? DeviceType { get; init; }

    public string Platform
    {
        get
        {
            if (DeviceType == null)
            {
                return Resources.Platform_NA;
            }

            var deviceInfo = Resources.ResourceManager.GetString($"Platform_{DeviceType}");

            return deviceInfo ?? Resources.Platform_Uncategorized;
        }
    }

    public decimal? MemberWin
    {
        get
        {
            if (!string.IsNullOrEmpty(ExtraInfo) && ExtraInfo.HasAttributeInXmlTag("p1", "pa") && this.IsRunningWaiting())
            {
                var payoutXml = ExtraInfo.GetXmlTagAttributeValue("p1", "pa");
                decimal.TryParse(payoutXml, out var payout);

                return payout - ActualStake;
            }

            return null;
        }
    }

    public string MemberWinText
    {
        get
        {
            var memberWin = MemberWin;

            return memberWin.HasValue ? ConvertByBetType.Winloss(memberWin.Value) : Resources.Platform_NA;
        }
    }
}