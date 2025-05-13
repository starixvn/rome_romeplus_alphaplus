using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Requests.Payments;

public class UpdateProviderRequest : BaseActionRequest
{
    public int ProviderId { get; set; }
    public int Status { get; set; }
    
    public List<ProviderSettingsModel> ProviderSettings { get; set; }
} 