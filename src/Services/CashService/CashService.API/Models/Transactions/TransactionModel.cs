namespace AlphaPlus.CashService.API.Models.Transactions;

public class TransactionModel
{
    public int TransactionId { get; set; }

    public string Username { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public int StatusId { get; set; }

    public string Status { get; set; }

    public decimal BetCredit { get; set; }

    public int TransactionTypeId { get; set; }

    public string TransactionType { get; set; }

    public string TransferRefNo { get; set; }

    public string Remark { get; set; }

    public DateTime CreatedTime { get; set; }

    public DateTime? LastUpdatedDate { get; set; }
}