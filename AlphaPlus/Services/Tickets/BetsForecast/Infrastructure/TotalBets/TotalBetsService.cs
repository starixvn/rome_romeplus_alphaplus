using AlphaPlus.BetsForecast.Domain.TotalBets;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Params;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets;

public interface ITotalBetsService
{
    Task<IEnumerable<HDPOULive>> GetHDPOULive(GetTotalBetsParam param);

    Task<IEnumerable<MoneyLine>> GetMoneyLine(GetTotalBetsParam param);

    Task<IEnumerable<OE1X2DND>> GetOE1X2DND(GetTotalBetsParam param);

    Task<IEnumerable<TotalGoal>> GetTotalGoal(GetTotalBetsParam param);

    Task<IEnumerable<Outright>> GetOutright(GetTotalBetsParam param);

    Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(GetTotalBetsParam param);

    Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(GetTotalBetsParam param);

    Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(GetTotalBetsParam param);

    Task<IEnumerable<NumberGame>> GetNumberGame(GetTotalBetsParam param);
}

public class TotalBetsService(
    IDynamicRepository repository,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : ITotalBetsService
{
    private readonly IDynamicRepository repository = repository;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public Task<IEnumerable<HDPOULive>> GetHDPOULive(GetTotalBetsParam param)
        => GetTotalBets<HDPOULive>(
            new GetHDPOULiveCriteria(param.CustLevel, param.CustId), param);

    public Task<IEnumerable<MoneyLine>> GetMoneyLine(GetTotalBetsParam param)
        => GetTotalBets<MoneyLine>(
            new GetMoneyLineCriteria(param.CustLevel, param.CustId), param);

    public Task<IEnumerable<OE1X2DND>> GetOE1X2DND(GetTotalBetsParam param)
        => GetTotalBets<OE1X2DND>(
            new GetOE1X2DNDCriteria(param.CustLevel, param.CustId), param);

    public Task<IEnumerable<TotalGoal>> GetTotalGoal(GetTotalBetsParam param)
        => GetTotalBets<TotalGoal>(
            new GetTotalGoalCriteria(param.CustLevel, param.CustId), param);

    public async Task<IEnumerable<Outright>> GetOutright(GetTotalBetsParam param)
    {
        var criteria = new GetOutrightCriteria(param.CustLevel, param.CustId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawdata = await repository.FetchAsync<Outright>(criteria);
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, param.Language);
            item.TeamName = cacheRefService.GetTeamName(item.TeamId, param.Language);
        }

        return rawdata;
    }

    public Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(GetTotalBetsParam param)
        => GetTotalBets<HalfTimeFullTime>(
            new GetHalfTimeFullTimeCriteria(param.CustLevel, param.CustId), param);

    public Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(GetTotalBetsParam param)
        => GetTotalBets<FirstGoalLastGoal>(
            new GetFirstGoalLastGoalCriteria(param.CustLevel, param.CustId), param);

    public Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(GetTotalBetsParam param)
        => GetTotalBets<HomeDrawAwayNoBet>(
            new GetHomeDrawAwayNoBetCriteria(param.CustLevel, param.CustId), param);

    public async Task<IEnumerable<NumberGame>> GetNumberGame(GetTotalBetsParam param)
    {
        var criteria = new GetNumberGameCriteria(param.CustLevel, param.CustId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawdata = await repository.FetchAsync<NumberGame>(criteria);
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, param.Language);
        }

        return rawdata;
    }

    private async Task<IEnumerable<T>> GetTotalBets<T>(ICriteria criteria, GetTotalBetsParam param) where T : TotalBetsMatch
    {
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var rawdata = await repository.FetchAsync<T>(criteria);
        foreach (var match in rawdata)
        {
            match.LeagueName = match.LeagueId == 0
                ? null
                : cacheRefService.GetLeagueName(match.LeagueId, param.Language);
            match.HomeName = cacheRefService.GetTeamName(match.HomeId, param.Language);
            match.AwayName = cacheRefService.GetTeamName(match.AwayId, param.Language);
        }

        return rawdata;
    }
}