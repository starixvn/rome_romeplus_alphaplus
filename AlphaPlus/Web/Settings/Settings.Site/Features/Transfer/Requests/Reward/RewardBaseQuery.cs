using AlphaPlus.Core.Utils;
using AlphaPlus.Core.Extensions;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;

public class RewardBaseQuery : TransferBaseQuery
{
    protected RewardBaseQuery()
    {
        SubAccId = viewer.SubAccId;
        LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        MinDate = SystemTime.Today.AddMonths(-3).FirstDayOfMonth().GetGreaterDate(LimitedDateToShowInfo);
        Language = viewer.Language;
    }

    public DateTime FromDate { get; set; } = SystemTime.Today;

    public DateTime ToDate { get; set; } = SystemTime.Today;

    public string ProgramIds { get; set; }

    public bool? PaymentStatus { get; set; }

    public string Username { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 500;

    public int SubAccId { get; }

    public DateTime LimitedDateToShowInfo { get; }

    public DateTime MinDate { get; }

    public string Language { get; }

    public override bool IsValid() => MinDate <= FromDate && FromDate <= ToDate && base.IsValid();
}