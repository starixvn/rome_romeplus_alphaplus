namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationEntity
{
    public int CustId { get; init; }

    public int CustLevel { get; init; }

    public string Username { get; init; }

    public byte CurrencyId { get; init; }

    public decimal Turnover { get; init; }

    public decimal GrossComm { get; init; }

    public decimal MemberWinLoss { get; init; }

    public decimal MemberComm { get; init; }

    public decimal MemberTotal { get; init; }

    public decimal OwnerTurnover { get; init; }

    public decimal OwnerWinLoss { get; init; }

    public decimal OwnerComm { get; init; }

    public decimal OwnerTotal { get; init; }

    public decimal Company { get; init; }

    public int BetCount { get; init; }
}