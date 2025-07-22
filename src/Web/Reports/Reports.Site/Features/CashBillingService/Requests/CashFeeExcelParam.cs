using AlphaPlus.Core.Utils;

namespace AlphaPlus.Reports.Site.Features.CashBillingService.Requests;

public class CashFeeExcelParam
{
    public DateTime FromDate { get; set; } = DateTime.MinValue;

    public DateTime ToDate { get; set; } = SystemTime.Today;

    public string CustName { get; set; }

}
