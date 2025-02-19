using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public interface ISoccerService
{
    Task<IEnumerable<League>> GetLeagues(bool isLive, string language, string siteName);

    Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language, string siteName);

    Task<IEnumerable<SoccerResult>> GetResults(int custId, string matchIds, bool isLive, string language, string siteName);
}

public class SoccerService(IMemoryCache cacheService, HttpClient client) : ISoccerService
{
    private const string SoccerUrl = "api/forecast/soccer";
    private const string LeaguesCacheKey = "AlphaPlus.Tickets.Site.SoccerLeagues_{0}_{1}";
    private const int LeaguesCacheTimeOut = 5; // 5 Minutes

    private readonly IMemoryCache cacheService = cacheService;
    private readonly HttpClient client = client;

    public Task<IEnumerable<League>> GetLeagues(bool isLive, string language, string siteName)
    {
        return cacheService.GetOrCreateAsync(
            string.Format(LeaguesCacheKey, isLive, language),
            cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(LeaguesCacheTimeOut));
                var isShowAll = Core.Configurations.AppSettings.LocalTest;
                var queryString = QueryString.Create(
                [
                    KeyValuePair.Create(nameof(isLive), isLive.ToString()),
                    KeyValuePair.Create(nameof(isShowAll), isShowAll.ToString()),
                    KeyValuePair.Create(nameof(language), language),
                    KeyValuePair.Create(nameof(siteName), siteName)
                ]);

                return client.GetJson<IEnumerable<League>>($"{SoccerUrl}/leagues{queryString}");
            });
    }

    public Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language, string siteName)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create(nameof(leagueId), leagueId.ToString()),
            KeyValuePair.Create(nameof(isLive), isLive.ToString()),
            KeyValuePair.Create(nameof(language), language),
            KeyValuePair.Create(nameof(siteName), siteName)
        ]);

        return client.GetJson<IEnumerable<Match>>($"{SoccerUrl}/matches{queryString}");
    }

    public Task<IEnumerable<SoccerResult>> GetResults(int custId, string matchIds, bool isLive, string language, string siteName)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(matchIds), matchIds),
            KeyValuePair.Create(nameof(isLive), isLive.ToString()),
            KeyValuePair.Create(nameof(language), language),
            KeyValuePair.Create(nameof(siteName), siteName)
        ]);

        return client.GetJson<IEnumerable<SoccerResult>>($"{SoccerUrl}/results{queryString}");
    }
}
