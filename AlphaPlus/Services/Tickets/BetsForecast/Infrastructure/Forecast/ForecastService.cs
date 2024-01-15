using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Domain.Shared;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Helper;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface IForecastService
{
    Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetSoccer(
        int custId,
        string matchIds,
        bool isLive);

    Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastParam param);

    Task<AH> GetAH(GetAHParam param);

    Task<(bool, decimal[])> GetHDP(GetAHParam param);

    Task<(bool, decimal[])> GetOU(GetAHParam param);

    Task<ForecastMatch> GetMatch(int matchId, int betType, string language);

    Task<IEnumerable<Forecast1X2>> Get1X2(GetForecastParam param);

    Task<IEnumerable<Forecast1X2>> Get1X2(Get1X2ByMatchParam param);
}

public class ForecastService : IForecastService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public ForecastService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetSoccer(
        int custId,
        string matchIds,
        bool isLive)
        => repository.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(new GetSoccerForecastCriteria(custId, matchIds, isLive));

    public Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(GetForecastParam param)
    {
        var langCode = LanguageHelper.GetLangCode(param.Language);
        ICriteria criteria = (UserLevel)param.CustLevel switch
        {
            UserLevel.Super => new GetHDPOULiveSuperCriteria(param.CustId, param.LeagueId, langCode),
            UserLevel.Master => new GetHDPOULiveMasterCriteria(param.CustId, param.LeagueId, langCode),
            UserLevel.Agent => new GetHDPOULiveAgentCriteria(param.CustId, param.LeagueId, langCode),
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(param.CustLevel)),
        };

        return GetForecast<ForecastHDPOULive>(criteria, param.Language);
    }

    public async Task<AH> GetAH(GetAHParam param)
    {
        var matchTask = GetMatch(param.MatchId, param.BetType, param.Language);
        var hdpouTask = IsHDP(param.BetType) ? GetHDP(param) : GetOU(param);
        await Task.WhenAll(matchTask, hdpouTask);

        var match = await matchTask;
        var (hasData, hdpou) = await hdpouTask;

        return new AH(hasData, hdpou)
        {
            MatchId = match.MatchId,
            HomeId = match.HomeId,
            HomeName = match.HomeName,
            AwayId = match.AwayId,
            AwayName = match.AwayName,
            IsNeutral = match.IsNeutral,
            LiveHomeScore = match.LiveHomeScore,
            LiveAwayScore = match.LiveAwayScore
        };
    }

    private static bool IsHDP(int betType)
        => betType == 1 || betType == 7 || betType == 301 || betType == 303;

    public async Task<(bool, decimal[])> GetHDP(GetAHParam param)
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
            hdp0 = GetHDPValue(item, param.CurrentScore, -2, hdp0);
            hdp1 = GetHDPValue(item, param.CurrentScore, -1, hdp1);
            hdp2 = GetHDPValue(item, param.CurrentScore, 0, hdp2);
            hdp3 = GetHDPValue(item, param.CurrentScore, 1, hdp3);
            hdp4 = GetHDPValue(item, param.CurrentScore, 2, hdp4);
        }

        return (rawdata.Any(), [hdp0, hdp1, hdp2, hdp3, hdp4]);
    }

    private static decimal GetHDPValue(HDPOU hdp, int currentScore, int scoreOffset, decimal forecast)
    {
        decimal forecastScore = currentScore - hdp.Offset + scoreOffset;

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

    public async Task<(bool, decimal[])> GetOU(GetAHParam param)
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
            ou0 = GetOUValue(item, param.CurrentScore, 0, ou0);
            ou1 = GetOUValue(item, param.CurrentScore, 1, ou1);
            ou2 = GetOUValue(item, param.CurrentScore, 2, ou2);
            ou3 = GetOUValue(item, param.CurrentScore, 3, ou3);
            ou4 = GetOUValue(item, param.CurrentScore, 4, ou4);
        }

        return (rawdata.Any(), [ou0, ou1, ou2, ou3, ou4]);
    }

    private static decimal GetOUValue(HDPOU ou, int currentScore, int scoreOffset, decimal forecast)
    {
        decimal forecastScore = currentScore - ou.Offset + scoreOffset;

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

    public async Task<ForecastMatch> GetMatch(int matchId, int betType, string language)
    {
        var match = await repository.GetAsync<ForecastMatch>(new GetMatchCriteria(matchId, betType));
        if (match != null)
        {
            match.HomeName = cacheRefService.GetTeamName(match.HomeId, language);
            match.AwayName = cacheRefService.GetTeamName(match.AwayId, language);
        }

        return match;
    }

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
