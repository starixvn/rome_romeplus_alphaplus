using AlphaPlus.Tickets.Site.Features.Shared.Extensions;

namespace AlphaPlus.Tickets.Site.Features.TotalBets;

public interface ITotalBetsService
{
    Task<IEnumerable<T>> GetTotalBets<T>(string action, byte custLevel, int custId, string language, string siteName);
}

public class TotalBetsService(HttpClient client) : ITotalBetsService
{
    private readonly HttpClient client = client;

    public Task<IEnumerable<T>> GetTotalBets<T>(string action, byte custLevel, int custId, string language, string siteName)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(language), language),
            KeyValuePair.Create(nameof(siteName), siteName)
        });

        return client.GetJson<IEnumerable<T>>($"api/totalbets/{action}{queryString}");
    }
}
