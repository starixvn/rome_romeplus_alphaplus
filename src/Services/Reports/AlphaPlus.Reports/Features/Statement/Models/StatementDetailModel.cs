namespace AlphaPlus.Reports.Features.Statement;

public class StatementDetailModel
{
    private readonly int[] _hideTransactionTypeBetTypes = new int[] { 31001, 31002, 31006 };

    public DateTime TransDate { get; init; }

    public string TransType { get; init; }

    public string TransDesc { get; init; }

    public decimal? Amount { get; set; }

    public string ExchangeRate { get; init; }

    public string Creator { get; init; }

    public int BetType { get; init; }

    public bool IsHiddenTransType => _hideTransactionTypeBetTypes.Contains(BetType);
}