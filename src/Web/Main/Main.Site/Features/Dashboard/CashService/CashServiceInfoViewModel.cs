using Fanex.Customer.Client.Models;

namespace AlphaPlus.Main.Site.Features.Dashboard.CashService;
public class CashServiceInfoViewModel
{
    public CashServiceInfoViewModel()
    {
        IsCashServiceEnabled = false;
        TotalCashMember = new TotalCashMember
        {
            TotalCashMemberActive = 0,
            TotalCashMemberSuspended = 0,
            TotalCashMemberClosed = 0,
        };
        IsOfferedCashService = false;
        IsCanUpdateCashServiceStatus = false;
        NickName = string.Empty;
        CDNUri = string.Empty;
    }
    public string NickName { get; set; }
    public bool IsCashServiceEnabled { get; set; }

    public TotalCashMember TotalCashMember { get; set; }

    public bool IsOfferedCashService { get; set; }

    public bool IsCanUpdateCashServiceStatus { get; set; }
    public bool IsCanEditCashServicePermission { get; set; }

    public string CDNUri { get; set; }
}
