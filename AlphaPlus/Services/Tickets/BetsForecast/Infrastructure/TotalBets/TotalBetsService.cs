using AlphaPlus.BetsForecast.Domain.TotalBets;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Params;
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

public class TotalBetsService : ITotalBetsService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public TotalBetsService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public Task<IEnumerable<HDPOULive>> GetHDPOULive(GetTotalBetsParam param)
        => GetTotalBets<HDPOULive>(
            new GetHDPOULiveCriteria(param.CustLevel, param.CustId), param.Language);

    public Task<IEnumerable<MoneyLine>> GetMoneyLine(GetTotalBetsParam param)
        => GetTotalBets<MoneyLine>(
            new GetMoneyLineCriteria(param.CustLevel, param.CustId), param.Language);

    public Task<IEnumerable<OE1X2DND>> GetOE1X2DND(GetTotalBetsParam param)
        => GetTotalBets<OE1X2DND>(
            new GetOE1X2DNDCriteria(param.CustLevel, param.CustId), param.Language);

    public Task<IEnumerable<TotalGoal>> GetTotalGoal(GetTotalBetsParam param)
        => GetTotalBets<TotalGoal>(
            new GetTotalGoalCriteria(param.CustLevel, param.CustId), param.Language);

    public async Task<IEnumerable<Outright>> GetOutright(GetTotalBetsParam param)
    {
        var rawdata = await repository.FetchAsync<Outright>(
            new GetOutrightCriteria(param.CustLevel, param.CustId));
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, param.Language);
            item.TeamName = cacheRefService.GetTeamName(item.TeamId, param.Language);
        }

        return rawdata;
    }

    public Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(GetTotalBetsParam param)
        => GetTotalBets<HalfTimeFullTime>(
            new GetHalfTimeFullTimeCriteria(param.CustLevel, param.CustId), param.Language);

    public Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(GetTotalBetsParam param)
        => GetTotalBets<FirstGoalLastGoal>(
            new GetFirstGoalLastGoalCriteria(param.CustLevel, param.CustId), param.Language);

    public Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(GetTotalBetsParam param)
        => GetTotalBets<HomeDrawAwayNoBet>(
            new GetHomeDrawAwayNoBetCriteria(param.CustLevel, param.CustId), param.Language);

    public async Task<IEnumerable<NumberGame>> GetNumberGame(GetTotalBetsParam param)
    {
        var rawdata = await repository.FetchAsync<NumberGame>(
            new GetNumberGameCriteria(param.CustLevel, param.CustId));
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, param.Language);
        }

        return rawdata;
    }

    private async Task<IEnumerable<T>> GetTotalBets<T>(ICriteria criteria, string language) where T : TotalBetsMatch
    {
        var rawdata = await repository.FetchAsync<T>(criteria);

        foreach (var match in rawdata)
        {
            match.LeagueName = match.LeagueId == 0
                ? null
                : cacheRefService.GetLeagueName(match.LeagueId, language);
            match.HomeName = cacheRefService.GetTeamName(match.HomeId, language);
            match.AwayName = cacheRefService.GetTeamName(match.AwayId, language);
        }

        return rawdata;
    }
}