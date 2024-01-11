using AlphaPlus.BetsForecast.Domain.TotalBets;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Helper;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets;

public interface ITotalBetsService
{
    Task<IEnumerable<HDPOULive>> GetHDPOULive(byte custLevel, int custId, string language);

    Task<IEnumerable<MoneyLine>> GetMoneyLine(byte custLevel, int custId, string language);

    Task<IEnumerable<OE1X2DND>> GetOE1X2DND(byte custLevel, int custId, string language);

    Task<IEnumerable<TotalGoal>> GetTotalGoal(byte custLevel, int custId, string language);

    Task<IEnumerable<Outright>> GetOutright(byte custLevel, int custId, string language);

    Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(byte custLevel, int custId, string language);

    Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(byte custLevel, int custId, string language);

    Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(byte custLevel, int custId, string language);

    Task<IEnumerable<NumberGame>> GetNumberGame(byte custLevel, int custId, string language);
}

public class TotalBetsService : ITotalBetsService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public TotalBetsService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public Task<IEnumerable<HDPOULive>> GetHDPOULive(byte custLevel, int custId, string language)
        => GetTotalBets<HDPOULive>(new GetHDPOULiveCriteria(custLevel, custId), language);

    public Task<IEnumerable<MoneyLine>> GetMoneyLine(byte custLevel, int custId, string language)
        => GetTotalBets<MoneyLine>(
            new GetMoneyLineCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)),
            language);

    public Task<IEnumerable<OE1X2DND>> GetOE1X2DND(byte custLevel, int custId, string language)
        => GetTotalBets<OE1X2DND>(
            new GetOE1X2DNDCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)),
            language);

    public Task<IEnumerable<TotalGoal>> GetTotalGoal(byte custLevel, int custId, string language)
        => GetTotalBets<TotalGoal>(
            new GetTotalGoalCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)),
            language);

    public async Task<IEnumerable<Outright>> GetOutright(byte custLevel, int custId, string language)
    {
        var rawdata = await repository.FetchAsync<Outright>(
            new GetOutrightCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)));
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
            item.TeamName = cacheRefService.GetTeamName(item.TeamId, language);
        }

        return rawdata;
    }

    public Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(byte custLevel, int custId, string language)
        => GetTotalBets<HalfTimeFullTime>(
            new GetHalfTimeFullTimeCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)),
            language);

    public Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(byte custLevel, int custId, string language)
        => GetTotalBets<FirstGoalLastGoal>(
            new GetFirstGoalLastGoalCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)),
            language);

    public Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(byte custLevel, int custId, string language)
        => GetTotalBets<HomeDrawAwayNoBet>(
            new GetHomeDrawAwayNoBetCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)),
            language);

    public async Task<IEnumerable<NumberGame>> GetNumberGame(byte custLevel, int custId, string language)
    {
        var rawdata = await repository.FetchAsync<NumberGame>(
            new GetNumberGameCriteria(custLevel, custId, LanguageHelper.GetLangCode(language)));
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
        }

        return rawdata;
    }

    private async Task<IEnumerable<T>> GetTotalBets<T>(ICriteria criteria, string language) where T : TotalBetsMatch
    {
        var rawdata = await repository.FetchAsync<T>(criteria);

        foreach (var match in rawdata)
        {
            match.LeagueName = match.LeagueId == 0
                ? string.Empty
                : cacheRefService.GetLeagueName(match.LeagueId, language);
            match.HomeName = $"{cacheRefService.GetTeamName(match.HomeId, language)}{(match.IsNeutral ? " (N)" : string.Empty)}";
            match.AwayName = cacheRefService.GetTeamName(match.AwayId, language);
        }

        return rawdata;
    }
}