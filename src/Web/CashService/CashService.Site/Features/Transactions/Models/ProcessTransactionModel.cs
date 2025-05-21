namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class ProcessTransactionModel
{
    public string TransId { get; set; }

    public decimal Amount { get; set; }

    public string Remark { get; set; }

    public IFormFile? Image { get; set; }

    public byte[] Payslip { get; set; }
}