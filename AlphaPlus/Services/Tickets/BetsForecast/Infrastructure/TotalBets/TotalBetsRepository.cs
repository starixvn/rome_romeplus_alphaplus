using AlphaPlus.BetsForecast.Domain.TotalBets;
using AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets;

public interface ITotalBetsRepository
{
    Task<IEnumerable<HDPOULive>> GetHDPOULive(byte custLevel, int custId);

    Task<IEnumerable<MoneyLine>> GetMoneyLine(byte custLevel, int custId, string language);

    Task<IEnumerable<OE1x2DND>> GetOE1x2DND(byte custLevel, int custId, string language);

    Task<IEnumerable<TotalGoal>> GetTotalGoal(byte custLevel, int custId, string language);

    Task<IEnumerable<Outright>> GetOutright(byte custLevel, int custId, string language);

    Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(byte custLevel, int custId, string language);

    Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(byte custLevel, int custId, string language);

    Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(byte custLevel, int custId, string language);

    Task<IEnumerable<NumberGame>> GetNumberGame(byte custLevel, int custId, string language);
}

public class TotalBetsRepository : ITotalBetsRepository
{
    private readonly IDynamicRepository repository;

    public TotalBetsRepository(IDynamicRepository repository)
        => this.repository = repository;

    public Task<IEnumerable<HDPOULive>> GetHDPOULive(byte custLevel, int custId)
        => repository.FetchAsync<HDPOULive>(new GetHDPOULiveCriteria(custLevel, custId));

    public Task<IEnumerable<MoneyLine>> GetMoneyLine(byte custLevel, int custId, string language)
        => repository.FetchAsync<MoneyLine>(new GetMoneyLineCriteria(custLevel, custId, language));

    public Task<IEnumerable<OE1x2DND>> GetOE1x2DND(byte custLevel, int custId, string language)
        => repository.FetchAsync<OE1x2DND>(new GetOE1x2DNDCriteria(custLevel, custId, language));

    public Task<IEnumerable<TotalGoal>> GetTotalGoal(byte custLevel, int custId, string language)
        => repository.FetchAsync<TotalGoal>(new GetTotalGoalCriteria(custLevel, custId, language));

    public Task<IEnumerable<Outright>> GetOutright(byte custLevel, int custId, string language)
        => repository.FetchAsync<Outright>(new GetOutrightCriteria(custLevel, custId, language));

    public Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(byte custLevel, int custId, string language)
        => repository.FetchAsync<HalfTimeFullTime>(new GetHalfTimeFullTimeCriteria(custLevel, custId, language));

    public Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(byte custLevel, int custId, string language)
        => repository.FetchAsync<FirstGoalLastGoal>(new GetFirstGoalLastGoalCriteria(custLevel, custId, language));

    public Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(byte custLevel, int custId, string language)
        => repository.FetchAsync<HomeDrawAwayNoBet>(new GetHomeDrawAwayNoBetCriteria(custLevel, custId, language));

    public Task<IEnumerable<NumberGame>> GetNumberGame(byte custLevel, int custId, string language)
        => repository.FetchAsync<NumberGame>(new GetNumberGameCriteria(custLevel, custId, language));
}
