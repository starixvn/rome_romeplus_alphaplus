using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.Features.Forecast.Requests;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public interface IForecastService
{
    Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language);

    Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastRequest message);

    Task<AH> GetAH(GetAHRequest message);

    Task<IEnumerable<Forecast1X2>> Get1X2(GetForecastRequest message);

    Task<IEnumerable<Forecast1X2>> Get1X2(GetForecast1X2ByMatchRequest message);
}

public class ForecastService : IForecastService
{
    private const string ForecastUrl = "api/forecast";

    private readonly HttpClient client;

    public ForecastService(HttpClient client) => this.client = client;

    public Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(betTypes), betTypes),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<League>>($"{ForecastUrl}/runningleagues{queryString}");
    }

    public async Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastRequest message)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.LeagueId), message.LeagueId.ToString()),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        return await client.GetFromJsonAsync<IEnumerable<ForecastHDPOULive>>($"{ForecastUrl}/hdpoulive{queryString}");
    }

    public Task<AH> GetAH(GetAHRequest message)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.MatchId), message.MatchId.ToString()),
            KeyValuePair.Create(nameof(message.BetType), message.BetType.ToString()),
            KeyValuePair.Create(nameof(message.Score), message.Score.ToString()),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        return client.GetFromJsonAsync<AH>($"{ForecastUrl}/ah{queryString}");
    }

    public Task<IEnumerable<Forecast1X2>> Get1X2(GetForecastRequest message)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.LeagueId), message.LeagueId.ToString()),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        return client.GetFromJsonAsync<IEnumerable<Forecast1X2>>($"{ForecastUrl}/1x2{queryString}");
    }

    public Task<IEnumerable<Forecast1X2>> Get1X2(GetForecast1X2ByMatchRequest message)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.MatchId), message.MatchId.ToString()),
            KeyValuePair.Create(nameof(message.BetType), message.BetType.ToString()),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        return client.GetFromJsonAsync<IEnumerable<Forecast1X2>>($"{ForecastUrl}/1x2bymatch{queryString}");
    }
}
