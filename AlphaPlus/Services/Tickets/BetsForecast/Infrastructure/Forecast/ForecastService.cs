using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface IForecastService
{
    Task<IEnumerable<League>> GetRunningLeagues(GetRunningLeaguesParam param);

    Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastParam param);

    Task<AH> GetAH(GetAHParam param);

    Task<IEnumerable<Forecast1X2>> Get1X2(GetForecastParam param);

    Task<IEnumerable<Forecast1X2>> Get1X2(Get1X2ByMatchParam param);
}

public class ForecastService(
    IDynamicRepository repository,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : IForecastService
{
    private readonly IDynamicRepository repository = repository;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<IEnumerable<League>> GetRunningLeagues(GetRunningLeaguesParam param)
    {
        ICriteria criteria;
        var custLevel = Enumeration.FromValue<UserLevel>(param.CustLevel);
        if (custLevel.IsSuperLevel())
        {
            criteria = new GetRunningLeagueSuperCriteria(param.CustId, param.BetTypes);
        }
        else if (custLevel.IsMasterLevel())
        {
            criteria = new GetRunningLeagueMasterCriteria(param.CustId, param.BetTypes);
        }
        else
        {
            criteria = new GetRunningLeagueAgentCriteria(param.CustId, param.BetTypes);
        }

        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var leagues = await repository.FetchAsync<League>(criteria);
        foreach (var item in leagues)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, param.Language);
        }

        return leagues;
    }

    public Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastParam param)
    {
        ICriteria criteria;
        var custLevel = Enumeration.FromValue<UserLevel>(param.CustLevel);
        if (custLevel.IsSuperLevel())
        {
            criteria = new GetHDPOULiveSuperCriteria(param.CustId, param.LeagueId);
        }
        else if (custLevel.IsMasterLevel())
        {
            criteria = new GetHDPOULiveMasterCriteria(param.CustId, param.LeagueId);
        }
        else
        {
            criteria = new GetHDPOULiveAgentCriteria(param.CustId, param.LeagueId);
        }

        return GetForecast<ForecastHDPOULive>(criteria, param.Language);
    }

    public async Task<AH> GetAH(GetAHParam param)
    {
        var isHandicap = IsHandicap(param.BetType);
        var hdpouTask = isHandicap ? GetAHHDP(param) : GetAHOU(param);
        var matchTask = GetAHMatch(param.MatchId, param.BetType, param.Language);
        await Task.WhenAll(matchTask, hdpouTask);

        var match = await matchTask;
        var (hasData, hdpou) = await hdpouTask;

        return match == null
            ? null
            : new AH(
                param.MatchId,
                match.Event,
                $"{match.LiveHomeScore} : {match.LiveAwayScore}",
                param.BetType,
                cacheRefService.GetBetTypeName(param.BetType, param.Language),
                param.Score,
                hasData,
                hdpou,
                GetAHAllScores(isHandicap, param.Score),
                GetAHForecastScores(isHandicap, param.Score));
    }

    private async Task<ForecastMatch> GetAHMatch(int matchId, int betType, string language)
    {
        var match = await repository.GetAsync<ForecastMatch>(new GetMatchCriteria(matchId, betType));
        if (match != null)
        {
            match.HomeName = cacheRefService.GetTeamName(match.HomeId, language);
            match.AwayName = cacheRefService.GetTeamName(match.AwayId, language);
        }

        return match;
    }

    private async Task<(bool, decimal[])> GetAHHDP(GetAHParam param)
    {
        var hdp0 = 0M;
        var hdp1 = 0M;
        var hdp2 = 0M;
        var hdp3 = 0M;
        var hdp4 = 0M;
        var rawdata = await repository.FetchAsync<HDPOU>(
            new GetHDPCriteria(param.CustLevel, param.CustId, param.MatchId, param.BetType));
        foreach (var item in rawdata)
        {
            hdp0 = GetAHHDPValue(item, param.Score, -2, hdp0);
            hdp1 = GetAHHDPValue(item, param.Score, -1, hdp1);
            hdp2 = GetAHHDPValue(item, param.Score, 0, hdp2);
            hdp3 = GetAHHDPValue(item, param.Score, 1, hdp3);
            hdp4 = GetAHHDPValue(item, param.Score, 2, hdp4);
        }

        return (rawdata.Any(), [hdp4, hdp3, hdp2, hdp1, hdp0]);
    }

    private static decimal GetAHHDPValue(HDPOU hdp, int score, int scoreOffset, decimal forecast)
    {
        decimal forecastScore = score - hdp.Offset + scoreOffset;

        if (forecastScore > 0 && forecastScore < 0.5M)
        {
            forecast += hdp.AL / 2; // win 1/2
        }
        else if (forecastScore >= 0.5M)
        {
            forecast += hdp.AL;
        }
        else if (forecastScore < 0 && forecastScore > -0.5M)
        {
            forecast += hdp.HL / 2;
        }
        else if (forecastScore <= -0.5M)
        {
            forecast += hdp.HL;
        }

        if (forecastScore == 0)
        {
            forecast += hdp.MYO;
        }

        return forecast;
    }

    private async Task<(bool, decimal[])> GetAHOU(GetAHParam param)
    {
        var ou0 = 0M;
        var ou1 = 0M;
        var ou2 = 0M;
        var ou3 = 0M;
        var ou4 = 0M;
        var rawdata = await repository.FetchAsync<HDPOU>(
            new GetOUCriteria(param.CustLevel, param.CustId, param.MatchId, param.BetType));
        foreach (var item in rawdata)
        {
            ou0 = GetAHOUValue(item, param.Score, 0, ou0);
            ou1 = GetAHOUValue(item, param.Score, 1, ou1);
            ou2 = GetAHOUValue(item, param.Score, 2, ou2);
            ou3 = GetAHOUValue(item, param.Score, 3, ou3);
            ou4 = GetAHOUValue(item, param.Score, 4, ou4);
        }

        return (rawdata.Any(), [ou4, ou3, ou2, ou1, ou0]);
    }

    private static decimal GetAHOUValue(HDPOU ou, int score, int scoreOffset, decimal forecast)
    {
        decimal forecastScore = score - ou.Offset + scoreOffset;

        if (forecastScore == 0.25M)
        {
            forecast += ou.AL / 2;
        }
        else if (forecastScore > 0.25M)
        {
            forecast += ou.AL;
        }
        else if (forecastScore == -0.25M)
        {
            forecast += ou.HL / 2;
        }
        else if (forecastScore < -0.25M)
        {
            forecast += ou.HL;
        }

        if (forecastScore == 0)
        {
            forecast += ou.MYO;
        }

        return forecast;
    }

    private static IEnumerable<int> GetAHAllScores(bool isHandicap, int score)
    {
        int maxIndex, endIndex;
        if (isHandicap)
        {
            maxIndex = 11;
            endIndex = score + 5;
        }
        else
        {
            maxIndex = 16;
            endIndex = 15;
        }

        for (int i = 0; i < maxIndex; i++)
        {
            yield return endIndex--;
        }
    }

    private static IEnumerable<int> GetAHForecastScores(bool isHandicap, int score)
    {
        int startIndex = isHandicap ? score - 2 : score;
        yield return startIndex + 4;
        yield return startIndex + 3;
        yield return startIndex + 2;
        yield return startIndex + 1;
        yield return startIndex;
    }

    private static bool IsHandicap(int betType)
        => betType == BetTypeConstants.Handicap || betType == BetTypeConstants._1stHandicap;

    public Task<IEnumerable<Forecast1X2>> Get1X2(GetForecastParam param)
        => GetForecast<Forecast1X2>(new Get1X2ByLeagueCriteria(param.CustLevel, param.CustId, param.LeagueId), param.Language);

    public Task<IEnumerable<Forecast1X2>> Get1X2(Get1X2ByMatchParam param)
        => GetForecast<Forecast1X2>(new Get1X2ByMatchCriteria(param.CustLevel, param.CustId, param.MatchId, param.BetType), param.Language);

    private async Task<IEnumerable<T>> GetForecast<T>(ICriteria criteria, string language) where T : ForecastMatch
    {
        var rawdata = await repository.FetchAsync<T>(criteria);
        foreach (var item in rawdata)
        {
            item.HomeName = cacheRefService.GetTeamName(item.HomeId, language);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId, language);
        }

        return rawdata;
    }
}
