using System.Data.SqlTypes;
using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.CashService.Site.Features.Promotion.Requests;

public class EditPromotionRequest : BaseActionRequest
{
    public int PromotionId { get; init; }
    public bool Enabled { get; init; }
    public bool? OriginalEnabled { get; init; }

    public List<PromotionLanguageSetting> Contents { get; set; }
    public List<PromotionLanguageSetting> OriginalContents { get; set; }
    
    public bool IsValid()
    {
        return true;
    }
}