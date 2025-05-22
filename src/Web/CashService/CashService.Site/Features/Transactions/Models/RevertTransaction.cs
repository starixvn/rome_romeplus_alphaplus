namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

public class RevertTransaction
{
    public long TransId { get; set; }
    public string Remark { get; set; }
    public int SiteDbGroupId { get; set; }
    public int UserId { get; set; }
    public string Ip { get; set; }
}