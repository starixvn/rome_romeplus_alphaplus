namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class AddTransactionParams
{
    public int CustId { get; set; }
    public int UplineId { get; set; }
    public int SubAccId { get; set; }
    public int ProviderId { get; set; }
    public int PaymentMethodId { get; set; }
    public decimal MinLimitAmt { get; set; }
    public decimal MaxLimitAmt { get; set; }
    public string PaymentFrom { get; set; }
    public string PaymentTo { get; set; }
    public decimal Amount { get; set; }
    public byte[] PaySlip { get; set; }
    public string Remark { get; set; }
    public string Ip { get; set; }
    public int SiteDbGroupId { get; set; }
    public int CustDbGroupId { get; set; }
    public IFormFile Image { get; set; }
}