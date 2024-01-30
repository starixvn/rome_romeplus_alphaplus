using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.Features.Shared;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Caching;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public interface ISoccerService
{
    Task<IEnumerable<League>> GetLeagues(bool isLive, string language);

    Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language);

    Task<IEnumerable<SoccerResult>> GetResults(int custId, string matchIds, bool isLive, string language);
}

public class SoccerService : ServiceBase, ISoccerService
{
    private const string SoccerUrl = "api/forecast/soccer";
    private const string LeaguesCacheKey = "AlphaPlus.Tickets.Site.SoccerLeagues_{0}_{1}";
    private const int LeaguesCacheTimeOut = 5; // 5 Minutes

    private readonly IOptions<AppSettings> options;
    private readonly ICacheService cacheService;

    public SoccerService(
        HttpClient client,
        IOptions<AppSettings> options,
        ICacheService cacheService) : base(client)
        => (this.options, this.cacheService) = (options, cacheService);

    public Task<IEnumerable<League>> GetLeagues(bool isLive, string language)
    {
        return cacheService.GetOrSetAsync(
            string.Format(LeaguesCacheKey, isLive, language),
            () =>
            {
                var isShowAll = options.Value.LocalTest;
                var queryString = QueryString.Create(new[]
                {
                    KeyValuePair.Create(nameof(isLive), isLive.ToString()),
                    KeyValuePair.Create(nameof(isShowAll), isShowAll.ToString()),
                    KeyValuePair.Create(nameof(language), language)
                });

                return GetFromJson<IEnumerable<League>>($"{SoccerUrl}/leagues{queryString}");
            },
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(LeaguesCacheTimeOut)));
    }

    public Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(leagueId), leagueId.ToString()),
            KeyValuePair.Create(nameof(isLive), isLive.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return GetFromJson<IEnumerable<Match>>($"{SoccerUrl}/matches{queryString}");
    }

    public Task<IEnumerable<SoccerResult>> GetResults(int custId, string matchIds, bool isLive, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(matchIds), matchIds),
            KeyValuePair.Create(nameof(isLive), isLive.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return GetFromJson<IEnumerable<SoccerResult>>($"{SoccerUrl}/results{queryString}");
    }
}
