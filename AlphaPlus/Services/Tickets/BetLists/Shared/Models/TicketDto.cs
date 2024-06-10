using AlphaPlus.BetLists.AppResources;
using Fanex.BetList.Core.Entities;

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
}