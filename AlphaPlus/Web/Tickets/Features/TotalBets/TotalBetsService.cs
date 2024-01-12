namespace AlphaPlus.Tickets.Site.Features.TotalBets;

public interface ITotalBetsService
{
    Task<IEnumerable<T>> GetTotalBets<T>(string action, byte custLevel, int custId, string language);
}

public class TotalBetsService : ITotalBetsService
{
    private readonly HttpClient client;

    public TotalBetsService(HttpClient client)
        => this.client = client;

    public Task<IEnumerable<T>> GetTotalBets<T>(string action, byte custLevel, int custId, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<T>>($"api/totalbets/{action}{queryString}");
    }
}