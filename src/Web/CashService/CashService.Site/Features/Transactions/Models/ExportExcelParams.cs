namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class ExportExcelParams
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string Account { get; set; }
    public int Status { get; set; }
    public int TransactionType { get; set; }
    public string Username { get; set; }
}