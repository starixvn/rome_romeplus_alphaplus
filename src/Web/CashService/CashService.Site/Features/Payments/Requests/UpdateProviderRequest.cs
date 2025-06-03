using AlphaPlus.CashService.Site.Features.Payments.Models;
using AlphaPlus.CashService.Site.Features.Shared.Requests;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests;

public class UpdateProviderRequest : BaseActionRequest
{
    public int ProviderId { get; set; }
    public int Status { get; set; }
    
    public List<ProviderSettingsModel> ProviderSettings { get; set; }
} 