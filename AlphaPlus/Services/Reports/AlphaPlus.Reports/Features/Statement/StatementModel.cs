namespace AlphaPlus.Reports.Features.Statement;

public class StatementModel
{
    public DateTime WinLossDate { get; init; }

    public int CustId { get; init; }

    public decimal? OpeningBalance { get; init; }

    public decimal? Turnover { get; init; }

    public decimal? WinLoss { get; init; }

    public decimal? Commission { get; init; }

    public decimal? TransferAmount { get; init; }

    public decimal? ClosingBalance { get; init; }

    public decimal? Outstanding { get; init; }

    public decimal? VoidAmount { get; init; }

    public decimal? GrandPrize { get; init; }

    public bool IsNotedBalance(StatementModel previousStatement)
        => previousStatement != null
        && previousStatement.ClosingBalance != OpeningBalance;
}