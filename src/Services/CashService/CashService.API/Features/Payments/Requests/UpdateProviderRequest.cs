using AlphaPlus.CashService.API.Features.Payments.Models;
using AlphaPlus.CashService.API.Features.Shared.Requests;

namespace AlphaPlus.CashService.API.Features.Payments.Requests;

public class UpdateProviderRequest : BaseActionRequest
{
    public int ProviderId { get; set; }
    public int Status { get; set; }
    
    public List<ProviderSettingsModel> ProviderSettings { get; set; }
} 