using AlphaPlus.Core.Utils;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;

public class TransferToDownlineHistoryQuery : TransferBaseQuery
{
    public TransferToDownlineHistoryQuery()
    {
        LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
    }

    public string UserName { get; set; }

    public DateTime FromDate { get; set; } = SystemTime.Today;

    public DateTime ToDate { get; set; } = SystemTime.Today;

    public int UserSelectedDateRangeOption { get; set; }

    public byte LevelType { get; set; }

    public DateTime LimitedDateToShowInfo { get; }
}
