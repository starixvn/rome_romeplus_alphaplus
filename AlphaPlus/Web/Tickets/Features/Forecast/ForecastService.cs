using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.Caching;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public interface IForecastService
{
    Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, string language);

    Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language);

    Task GetSoccer(int custId, string matchIds, bool isLive, string language);

    Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language);

    Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(byte custLevel, int custId, int leagueId, string language);

    Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int leagueId, string language);

    Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int matchId, int betType, string language);

    Task<IEnumerable<ScoreMapBetType>[]> GetScoreMapBetTypes(string language);

    Task<IEnumerable<League>> GetScoreMapLeagues(int custId, string language);

    Task<IEnumerable<Match>> GetScoreMapMatches(int custId, int leagueId, string language);

    Task<IEnumerable<ScoreMapResult>> GetScoreMapResult(
        byte custLevel,
        int custId,
        string matchIds,
        string liveBetTypes,
        string deadBetTypes,
        string language);
}

public class ForecastService : IForecastService
{
    private const string ForecastUrl = "api/forecast";
    private const string LeaguesCacheKey = "AlphaPlus.Tickets.Site.SoccerLeagues_{0}_{1}";
    private const int LeaguesCacheTimeOut = 5; // 5 Minutes
    private const int HalfTimeMinutes = 45;
    private const string ScoreMapBetTypesCacheKey = "AlphaPlus.Tickets.Site.ScoreMapBetTypes_{0}";
    private const int ScoreMapBetTypesCacheTimeOut = 2; // 2 hours

    private readonly HttpClient client;
    private readonly IOptions<AppSettings> options;
    private readonly ICacheService cacheService;

    public ForecastService(
        HttpClient client,
        IOptions<AppSettings> options,
        ICacheService cacheService)
        => (this.client, this.options, this.cacheService) = (client, options, cacheService);

    public Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, string language)
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

                return client.GetFromJsonAsync<IEnumerable<League>>($"{ForecastUrl}/soccerleagues{queryString}");
            },
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(LeaguesCacheTimeOut)));
    }

    public Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(leagueId), leagueId.ToString()),
            KeyValuePair.Create(nameof(isLive), isLive.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<Match>>($"{ForecastUrl}/soccermatches{queryString}");
    }

    public async Task GetSoccer(int custId, string matchIds, bool isLive, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(matchIds), matchIds),
            KeyValuePair.Create(nameof(isLive), isLive.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });
        var result = await client.GetFromJsonAsync<Tuple<IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>>>($"{ForecastUrl}/soccer{queryString}");
        var (matches, betTypes, winloss) = result;

        if (matches.Any())
        {
            var betTypesLookup = betTypes.ToLookup(b => b.MatchId);
            var winlossLookup = winloss.ToLookup(w => w.MatchId);

            foreach (var item in matches)
            {
                var betTypeList = betTypesLookup[item.MatchId].OrderBy(b => b.BetTypeId);
                var winLossList = winlossLookup[item.MatchId].OrderBy(w => w.BetTypeId);

                //matchForecasts.Add(BuildMatchForecast(match, betTypes, winLossNames));
            }
        }

        throw new NotImplementedException();
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

        return client.GetFromJsonAsync<IEnumerable<League>>($"{ForecastUrl}/runningleagues{queryString}");
    }

    public Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(byte custLevel, int custId, int leagueId, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(leagueId), leagueId.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<ForecastHDPOULive>>($"{ForecastUrl}/hdpoulive{queryString}");
    }

    public Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int leagueId, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(leagueId), leagueId.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<Forecast1X2>>($"{ForecastUrl}/1x2{queryString}");
    }

    public Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int matchId, int betType, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(matchId), matchId.ToString()),
            KeyValuePair.Create(nameof(betType), betType.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<Forecast1X2>>($"{ForecastUrl}/1x2bymatch{queryString}");
    }

    public Task<IEnumerable<ScoreMapBetType>[]> GetScoreMapBetTypes(string language)
        => cacheService.GetOrSetAsync(
            string.Format(ScoreMapBetTypesCacheKey, language),
            async () =>
            {
                var queryString = QueryString.Create(nameof(language), language);
                var rawdata = await client.GetFromJsonAsync<IEnumerable<ScoreMapBetType>[]>($"{ForecastUrl}/scoremap/bettypes{queryString}");
                UpdateBetTypeName(rawdata[0]);
                UpdateBetTypeName(rawdata[1]);

                return rawdata;
            },
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromHours(ScoreMapBetTypesCacheTimeOut)));

    private static void UpdateBetTypeName(IEnumerable<ScoreMapBetType> scoreMapBetTypes)
    {
        foreach (var item in scoreMapBetTypes)
        {
            if (item.BetType == 5)
            {
                item.BetTypeName = Resources._1X2;
            }
        }
    }

    public Task<IEnumerable<League>> GetScoreMapLeagues(int custId, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<League>>($"{ForecastUrl}/scoremap/leagues{queryString}");
    }

    public Task<IEnumerable<Match>> GetScoreMapMatches(int custId, int leagueId, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(leagueId), leagueId.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<Match>>($"{ForecastUrl}/scoremap/matches{queryString}");
    }

    public Task<IEnumerable<ScoreMapResult>> GetScoreMapResult(
        byte custLevel,
        int custId,
        string matchIds,
        string liveBetTypes,
        string deadBetTypes,
        string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custLevel), custLevel.ToString()),
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(matchIds), matchIds),
            KeyValuePair.Create(nameof(liveBetTypes), liveBetTypes),
            KeyValuePair.Create(nameof(deadBetTypes), deadBetTypes),
            KeyValuePair.Create(nameof(language), language)
        });

        return client.GetFromJsonAsync<IEnumerable<ScoreMapResult>>($"{ForecastUrl}/scoremap/result{queryString}");
    }
}
