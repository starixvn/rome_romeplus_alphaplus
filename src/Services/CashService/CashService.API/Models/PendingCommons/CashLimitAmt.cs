namespace AlphaPlus.CashService.API.Models.PendingCommons;

public class CashLimitAmt
{
    public int CustId { get; set; }
    public int ProviderId { get; set; }
    public int UplineId { get; set; }
    public int CustDbGroupId { get; set; }
    public int SiteDbGroupId { get; set; }
}