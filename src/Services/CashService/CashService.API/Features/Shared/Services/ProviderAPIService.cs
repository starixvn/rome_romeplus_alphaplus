
namespace AlphaPlus.CashService.API.Features.Shared.Services;

public interface IProviderAPIService
{
    Task<int> GetStatus(int providerId);
}

public class ProviderAPIService : IProviderAPIService
{
    public async Task<int> GetStatus(int providerId)
    {
        return 2;
    }
}