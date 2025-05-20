namespace AlphaPlus.CashService.API.Models.Transactions;

public class ProcessTransactionModel
{
    public string TransactionId { get; set; }
    public string PaymentFrom { get; set; }
    public string Remark { get; set; }
    public byte[] Payslip { get; set; }
    public int StatusId { get; set; }
}