namespace AlphaPlus.Reports.Features.Statement;

public class StatementDetailModel
{
    public DateTime TransDate { get; init; }

    public string TransType { get; init; }

    public string TransDesc { get; init; }

    public decimal? Amount { get; set; }

    public string ExchangeRate { get; init; }

    public string Creator { get; init; }
}