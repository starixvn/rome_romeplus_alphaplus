using AlphaPlus.BetsForecast.Domain.Forecast;
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
}

public class ForecastService : IForecastService
{
    private const string ForecastUrl = "api/forecast";
    private const string LeaguesCachingKey = "AlphaPlus.Tickets.Site.SoccerLeagues_{0}_{1}";
    private const int LeaguesCacheTimeOut = 5; // 5 Minutes
    private const int HalfTimeMinutes = 45;

    private readonly HttpClient client;
    private readonly IOptions<AppSettings> options;
    private readonly ICacheService cacheService;

    public ForecastService(
        HttpClient client,
        IOptions<AppSettings> options,
        ICacheService cacheService)
        => (this.client, this.options, this.cacheService) = (client, options, cacheService);

    public Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, string language)
        => cacheService.GetOrSetAsync(
            string.Format(LeaguesCachingKey, isLive, language),
            () => client.GetFromJsonAsync<IEnumerable<League>>(
                    $"{ForecastUrl}/soccerleagues?isLive={isLive}&isShowAll={options.Value.LocalTest}&language={language}"),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(LeaguesCacheTimeOut)));


    public Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language)
        => client.GetFromJsonAsync<IEnumerable<Match>>(
            $"{ForecastUrl}/soccermatches?leagueId={leagueId}&isLive={isLive}&language={language}");

    public async Task GetSoccer(int custId, string matchIds, bool isLive, string language)
    {
        var result = await client.GetFromJsonAsync<Tuple<IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>>>(
            $"{ForecastUrl}/soccer?custId={custId}&matchIds={matchIds}&isLive={isLive}&language={language}");
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
        => client.GetFromJsonAsync<IEnumerable<League>>(
            $"{ForecastUrl}/runningleagues?custLevel={custLevel}&custId={custId}&betTypes={betTypes}&language={language}");

    public Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(byte custLevel, int custId, int leagueId, string language)
        => client.GetFromJsonAsync<IEnumerable<ForecastHDPOULive>>(
            $"{ForecastUrl}/hdpoulive?custLevel={custLevel}&custId={custId}&leagueId={leagueId}&language={language}");

    public Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int leagueId, string language)
        => client.GetFromJsonAsync<IEnumerable<Forecast1X2>>(
            $"{ForecastUrl}/1x2?custLevel={custLevel}&custId={custId}&leagueId={leagueId}&language={language}");

    public Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int matchId, int betType, string language)
        => client.GetFromJsonAsync<IEnumerable<Forecast1X2>>(
            $"{ForecastUrl}/1x2bymatch?custLevel={custLevel}&custId={custId}&matchId={matchId}&betType={betType}&language={language}");
}
