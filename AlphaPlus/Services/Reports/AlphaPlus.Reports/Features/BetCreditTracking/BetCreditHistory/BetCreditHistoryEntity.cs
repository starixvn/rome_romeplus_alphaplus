using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

public class BetCreditHistoryEntity
{
    public DateTime ActionDate { get; init; }

    public DateTime? WinLostDate { get; init; }

    public long? RefNo { get; init; }

    public string RefNoString => RefNo?.ToString() ?? string.Empty;

    public decimal? ActualStake { get; init; }

    public decimal? WinLoss { get; init; }

    public decimal? PlayerComm { get; init; }

    public decimal? BalanceChanged { get; init; }

    public decimal? NewBalance { get; init; }

    public decimal GivenCredit { get; init; }

    public decimal? BetCredit { get; init; }

    public string Action { get; set; }

    public int ActionId { get; init; }

    public byte IsUserLogActionId { get; init; }

    public string ActionDateString => ActionDate.ToLongDate24HourWithSlash();

    public string WinLostDateString
        => WinLostDate.HasValue ? WinLostDate.Value.ToShortDateWithSlash() : string.Empty;
}