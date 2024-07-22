namespace AlphaPlus.Settings.Transfer.API.Models.TransferYesterdayBalance;

public class MemberTransfer
{
    public int CustId { get; init; }

    public string Username { get; init; }

    public decimal CashBalance { get; init; }

    public decimal Oustanding { get; init; }

    public decimal GivenCredit { get; init; }

    public decimal BetCredit { get; init; }

    public decimal AmountTransferred { get; init; }

    public DateTime LastLogin { get; init; }

    public DateTime MaxTransDate { get; init; }

    public decimal Balance { get; init; }

    public decimal PendingTransfer3rd { get; init; }
}