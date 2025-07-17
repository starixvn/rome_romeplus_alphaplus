using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class EditPromotionRequest : BaseActionRequest
{
    public int PromotionId { get; init; }
    public bool Enabled { get; init; }

    public List<Object> Contents { get; set; }
    public bool IsValid()
    {
        return true;
    }
}