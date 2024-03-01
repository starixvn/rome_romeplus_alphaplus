using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.Features.Forecast.Requests;
using AlphaPlus.Tickets.Site.Features.Shared;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public interface IForecastService
{
    Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language);

    Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastRequest message, CancellationToken cancellation);

    Task<AH> GetAH(GetAHRequest message, CancellationToken cancellation);

    Task<IEnumerable<Forecast1X2>> Get1X2(GetForecastRequest message, CancellationToken cancellation);

    Task<IEnumerable<Forecast1X2>> Get1X2(GetForecast1X2ByMatchRequest message, CancellationToken cancellation);
}

public class ForecastService : ServiceBase, IForecastService
{
    private const string ForecastUrl = "api/forecast";

    public ForecastService(HttpClient client) : base(client)
    {
    }

    public Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(betTypes), betTypes),
            KeyValuePair.Create(nameof(language), language)
        });

        return GetFromJson<IEnumerable<League>>($"{ForecastUrl}/runningleagues{queryString}");
    }

    public Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastRequest message, CancellationToken cancellation)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.LeagueId), message.LeagueId.ToString()),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        return GetFromJson<IEnumerable<ForecastHDPOULive>>($"{ForecastUrl}/hdpoulive{queryString}");
    }

    public Task<AH> GetAH(GetAHRequest message, CancellationToken cancellation)
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

        return GetFromJson<AH>($"{ForecastUrl}/ah{queryString}");
    }

    public Task<IEnumerable<Forecast1X2>> Get1X2(GetForecastRequest message, CancellationToken cancellation)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.LeagueId), message.LeagueId.ToString()),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        return GetFromJson<IEnumerable<Forecast1X2>>($"{ForecastUrl}/1x2{queryString}");
    }

    public Task<IEnumerable<Forecast1X2>> Get1X2(GetForecast1X2ByMatchRequest message, CancellationToken cancellation)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.MatchId), message.MatchId.ToString()),
            KeyValuePair.Create(nameof(message.BetType), message.BetType.ToString()),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        return GetFromJson<IEnumerable<Forecast1X2>>($"{ForecastUrl}/1x2bymatch{queryString}");
    }
}
