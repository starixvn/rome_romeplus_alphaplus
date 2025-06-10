namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo;
using Fanex.Customer.Client.Models;

public class CashServiceInfo
{
    public bool IsCashServiceEnabled { get; set; }
    
    public TotalCashMember TotalCashMember { get; set; }
    
    public bool IsOfferedCashService { get; set; }
}
