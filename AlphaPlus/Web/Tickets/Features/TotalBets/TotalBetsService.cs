using AlphaPlus.Tickets.Site.Features.Shared;

namespace AlphaPlus.Tickets.Site.Features.TotalBets;

public interface ITotalBetsService
{
    Task<IEnumerable<T>> GetTotalBets<T>(string action, byte custLevel, int custId, string language, string siteName);
}

public class TotalBetsService : ServiceBase, ITotalBetsService
{
    public TotalBetsService(HttpClient client) : base(client)
    {
    }

    public Task<IEnumerable<T>> GetTotalBets<T>(string action, byte custLevel, int custId, string language, string siteName)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(language), language),
            KeyValuePair.Create(nameof(siteName), siteName)
        });

        return GetFromJson<IEnumerable<T>>($"api/totalbets/{action}{queryString}");
    }
}