using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.App_GlobalResources;
using AlphaPlus.Tickets.Site.Features.Forecast.Requests;
using AlphaPlus.Tickets.Site.Features.Shared;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using Fanex.Caching;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public interface IScoreMapService
{
    Task<IEnumerable<ScoreMapBetType>[]> GetBetTypes(string language, string siteName);

    Task<IEnumerable<League>> GetLeagues(int custId, string language, string siteName);

    Task<IEnumerable<Match>> GetMatches(int custId, int leagueId, string language);

    Task<IEnumerable<ScoreMapResult>> GetResults(GetScoreMapResultRequest message, CancellationToken cancellation);
}

public class ScoreMapService : ServiceBase, IScoreMapService
{
    private const string ScoreMapUrl = "api/forecast/scoremap";
    private const string BetTypesCacheKey = "AlphaPlus.Tickets.Site.ScoreMapBetTypes_{0}";
    private const int BetTypesCacheTimeOut = 2; // 2 hours

    private readonly ICacheService cacheService;

    public ScoreMapService(HttpClient client, ICacheService cacheService) : base(client)
        => this.cacheService = cacheService;

    public Task<IEnumerable<ScoreMapBetType>[]> GetBetTypes(string language, string siteName)
        => cacheService.GetOrSetAsync(
            string.Format(BetTypesCacheKey, language),
            async () =>
            {
                var queryString = QueryString.Create(new[]
                {
                    KeyValuePair.Create(nameof(language), language),
                    KeyValuePair.Create(nameof(siteName), siteName)
                });

                var rawdata = await GetFromJson<IEnumerable<ScoreMapBetType>[]>($"{ScoreMapUrl}/bettypes{queryString}");
                UpdateBetTypeName(rawdata[0]);
                UpdateBetTypeName(rawdata[1]);

                return rawdata;
            },
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromHours(BetTypesCacheTimeOut)));

    private static void UpdateBetTypeName(IEnumerable<ScoreMapBetType> betTypes)
    {
        foreach (var item in betTypes)
        {
            if (item.BetType == BetTypeConstants._1X2)
            {
                item.BetTypeName = Resources._1X2;
            }
        }
    }

    public Task<IEnumerable<League>> GetLeagues(int custId, string language, string siteName)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(language), language),
            KeyValuePair.Create(nameof(siteName), siteName)
        });

        return GetFromJson<IEnumerable<League>>($"{ScoreMapUrl}/leagues{queryString}");
    }

    public Task<IEnumerable<Match>> GetMatches(int custId, int leagueId, string language)
    {
        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(leagueId), leagueId.ToString()),
            KeyValuePair.Create(nameof(language), language)
        });

        return GetFromJson<IEnumerable<Match>>($"{ScoreMapUrl}/matches{queryString}");
    }

    public async Task<IEnumerable<ScoreMapResult>> GetResults(GetScoreMapResultRequest message, CancellationToken cancellation)
    {
        var betTypes = await GetBetTypes(message.Language, message.SiteName);
        var liveBetTypes = BuildFollowedBetTypes(message.LiveBetTypes, betTypes[0]).Join();
        var deadBetTypes = BuildFollowedBetTypes(message.DeadBetTypes, betTypes[1]).Join();

        var queryString = QueryString.Create(new[]
        {
            KeyValuePair.Create("custLevel", message.ViewerLevel.ToString()),
            KeyValuePair.Create("custId", message.ViewerId.ToString()),
            KeyValuePair.Create(nameof(message.MatchIds), message.MatchIds),
            KeyValuePair.Create(nameof(liveBetTypes), liveBetTypes),
            KeyValuePair.Create(nameof(deadBetTypes), deadBetTypes),
            KeyValuePair.Create(nameof(message.Language), message.Language)
        });

        var rawdata = await GetFromJson<IEnumerable<ScoreMapResultDTO>>($"{ScoreMapUrl}/results{queryString}");

        return BuildResults(rawdata);
    }

    private static IEnumerable<string> BuildFollowedBetTypes(string ids, IEnumerable<ScoreMapBetType> betTypes)
    {
        if (!string.IsNullOrWhiteSpace(ids))
        {
            var arrayIds = ids.ToListInt();
            var betTypeHash = betTypes.ToDictionary(kvp => kvp.BetType, kvp => kvp.FollowedBetTypes);

            foreach (var item in arrayIds)
            {
                if (betTypeHash.TryGetValue(item, out var followedBetTypes))
                {
                    yield return followedBetTypes;
                }
            }
        }
    }

    private static IEnumerable<ScoreMapResult> BuildResults(IEnumerable<ScoreMapResultDTO> rawdata)
    {
        const int DefaultRowColumn = 7;
        int Round(decimal value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
        var scoreMapResults = new List<ScoreMapResult>();
        foreach (var match in rawdata)
        {
            var ht = new int[DefaultRowColumn, DefaultRowColumn];
            var ft = new int[DefaultRowColumn, DefaultRowColumn];

            foreach (var item in match.ScoreMapResults)
            {
                if (item.HT.HasValue)
                {
                    ht[item.Away, item.Home] = Round(item.HT.Value);
                }
                else
                {
                    ht = null;
                }

                if (item.FT.HasValue)
                {
                    ft[item.Away, item.Home] = Round(item.FT.Value);
                }
                else
                {
                    ft = null;
                }
            }

            scoreMapResults.Add(new ScoreMapResult(DefaultRowColumn, DefaultRowColumn, ht, ft)
            {
                MatchId = match.MatchId,
                HomeId = match.HomeId,
                HomeName = match.HomeName,
                AwayId = match.AwayId,
                AwayName = match.AwayName
            });
        }

        return scoreMapResults;
    }
}