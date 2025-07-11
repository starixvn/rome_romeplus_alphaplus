using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class AddPromotionRequest : BaseActionRequest
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; init; }
    public int PromotionType { get; init; }
    public bool Enabled { get; init; }

    public List<Object> ProductSettings{ get; set; }

    public List<Object> Contents { get; set; }

    public List<int> ProductIds { get; set; }
    public bool IsValid()
    {
        return SqlDateTime.MinValue.Value < FromDate && FromDate <= ToDate;
    }
}