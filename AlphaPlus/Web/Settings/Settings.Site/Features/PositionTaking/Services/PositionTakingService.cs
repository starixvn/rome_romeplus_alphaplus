using AlphaPlus.Settings.Site.Features.PositionTaking.Requests;

namespace AlphaPlus.Settings.Site.Features.PositionTaking.Services;

public interface IPositionTakingService
{
    Task<dynamic> GetPT(PositionTakingRequest request);
}

public class PositionTakingService(HttpClient httpClient) : IPositionTakingService
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<dynamic> GetPT(PositionTakingRequest request)
    {
        var param = QueryString.Create(new[] {
            KeyValuePair.Create(nameof(request.RoleId), request.RoleId.ToString()),
            KeyValuePair.Create(nameof(request.CustId), request.CustId.ToString()),
            KeyValuePair.Create(nameof(request.ProductId), request.ProductId.ToString()),
            KeyValuePair.Create(nameof(request.PageSize), request.PageSize.ToString()),
            KeyValuePair.Create(nameof(request.PageIndex), request.PageIndex.ToString()),
            KeyValuePair.Create(nameof(request.Language), request.Language),
            KeyValuePair.Create(nameof(request.ProductGroupId), request.ProductGroupId.ToString())
        });

        return await httpClient.GetAsJsonAsync<dynamic>($"customer/position-taking{param}");
    }
}