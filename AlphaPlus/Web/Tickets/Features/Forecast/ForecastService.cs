using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using AlphaPlus.Tickets.Site.Features.Shared.Services;
using Fanex.Caching;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Forecast;

public interface IForecastService
{
    Task<IEnumerable<League>> GetSoccerLeagues(bool isLive);

    Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive);

    Task GetSoccer(int custId, string matchIds, bool isLive);
}

public class ForecastService : IForecastService
{
    private const string LeaguesCachingKey = "AlphaPlus.Tickets.Site.SoccerLeagues_{0}";
    private const int LeaguesCacheTimeOut = 5; // 5 Minutes
    private const int HalfTimeMinutes = 45;

    private readonly HttpClient client;
    private readonly IOptions<AppSettings> options;
    private readonly ICacheService cacheService;
    private readonly ICacheRefService cacheRefService;

    public ForecastService(
        HttpClient client,
        IOptions<AppSettings> options,
        ICacheService cacheService,
        ICacheRefService cacheRefService)
        => (this.client, this.options, this.cacheService, this.cacheRefService) = (client, options, cacheService, cacheRefService);

    public Task<IEnumerable<League>> GetSoccerLeagues(bool isLive)
        => cacheService.GetOrSetAsync(
            string.Format(LeaguesCachingKey, isLive),
            async () =>
            {
                var leagues = await client.GetFromJsonAsync<IEnumerable<League>>(
                    $"api/forecast/soccerleagues?isLive={isLive}&isShowAll={options.Value.LocalTest}");
                foreach (var item in leagues)
                {
                    item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId);
                }

                return leagues;
            }, new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(LeaguesCacheTimeOut)));


    public async Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive)
    {
        var matches = await client.GetFromJsonAsync<IEnumerable<Match>>(
            $"api/forecast/soccermatches?leagueId={leagueId}&isLive={isLive}");

        foreach (var item in matches)
        {
            item.HomeName = cacheRefService.GetTeamName(item.HomeId);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId);
        }

        return matches;
    }

    public async Task GetSoccer(int custId, string matchIds, bool isLive)
    {
        var result = await client.GetFromJsonAsync<Tuple<IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>>>(
            $"api/forecast/soccer?custId={custId}&matchIds={matchIds}&isLive={isLive}");
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
}