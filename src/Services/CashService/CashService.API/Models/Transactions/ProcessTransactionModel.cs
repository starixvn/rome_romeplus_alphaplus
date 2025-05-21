namespace AlphaPlus.CashService.API.Models.Transactions;

public class ProcessTransactionModel
{
    public string TransId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentFrom { get; set; }

    public string Remark { get; set; }

    public byte[] Payslip { get; set; }

    public int StatusId { get; set; }
}