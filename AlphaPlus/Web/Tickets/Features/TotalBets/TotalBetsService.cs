using AlphaPlus.BetsForecast.Domain.TotalBets;
using AlphaPlus.Tickets.Site.Features.Shared.Services;

namespace AlphaPlus.Tickets.Site.Features.TotalBets;

public interface ITotalBetsService
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

public class TotalBetsService : ITotalBetsService
{
    private const string TotalBetsUrl = "api/totalbets";
    private readonly HttpClient client;
    private readonly ICacheRefService cacheRefService;

    public TotalBetsService(HttpClient client, ICacheRefService cacheRefService)
        => (this.client, this.cacheRefService) = (client, cacheRefService);

    public Task<IEnumerable<HDPOULive>> GetHDPOULive(byte custLevel, int custId)
        => GetTotalBets<HDPOULive>($"{TotalBetsUrl}/hdpoulive?custlevel={custLevel}&custid={custId}");

    public Task<IEnumerable<MoneyLine>> GetMoneyLine(byte custLevel, int custId, string language)
        => GetTotalBets<MoneyLine>($"{TotalBetsUrl}/moneyline?custlevel={custLevel}&custid={custId}&language={language}");

    public Task<IEnumerable<OE1x2DND>> GetOE1x2DND(byte custLevel, int custId, string language)
        => GetTotalBets<OE1x2DND>($"{TotalBetsUrl}/oe1x2dnd?custlevel={custLevel}&custid={custId}&language={language}");

    public Task<IEnumerable<TotalGoal>> GetTotalGoal(byte custLevel, int custId, string language)
        => GetTotalBets<TotalGoal>($"{TotalBetsUrl}/totalgoal?custlevel={custLevel}&custid={custId}&language={language}");

    public async Task<IEnumerable<Outright>> GetOutright(byte custLevel, int custId, string language)
    {
        var rawdata = await client.GetFromJsonAsync<IEnumerable<Outright>>(
            $"{TotalBetsUrl}/outright?custlevel={custLevel}&custid={custId}&language={language}");
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId);
            item.TeamName = cacheRefService.GetTeamName(item.TeamId);
        }

        return rawdata;
    }

    public Task<IEnumerable<HalfTimeFullTime>> GetHalfTimeFullTime(byte custLevel, int custId, string language)
        => GetTotalBets<HalfTimeFullTime>($"{TotalBetsUrl}/htft?custlevel={custLevel}&custid={custId}&language={language}");

    public Task<IEnumerable<FirstGoalLastGoal>> GetFirstGoalLastGoal(byte custLevel, int custId, string language)
        => GetTotalBets<FirstGoalLastGoal>($"{TotalBetsUrl}/fglg?custlevel={custLevel}&custid={custId}&language={language}");

    public Task<IEnumerable<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBet(byte custLevel, int custId, string language)
        => GetTotalBets<HomeDrawAwayNoBet>($"{TotalBetsUrl}/homedrawawaynobet?custlevel={custLevel}&custid={custId}&language={language}");

    public async Task<IEnumerable<NumberGame>> GetNumberGame(byte custLevel, int custId, string language)
    {
        var rawdata = await client.GetFromJsonAsync<IEnumerable<NumberGame>>(
            $"{TotalBetsUrl}/numbergame?custlevel={custLevel}&custid={custId}&language={language}");
        foreach (var item in rawdata)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId);
        }

        return rawdata;
    }

    private async Task<IEnumerable<T>> GetTotalBets<T>(string requestUri) where T : TotalBetsMatch
    {
        var totalBets = await client.GetFromJsonAsync<IEnumerable<T>>(requestUri);

        foreach (var item in totalBets)
        {
            item.LeagueName = item.LeagueId == 0 ? string.Empty : cacheRefService.GetLeagueName(item.LeagueId);
            item.HomeName = string.Format(
                "{0}{1}",
                cacheRefService.GetTeamName(item.HomeId),
                item.IsNeutral ? " (N)" : string.Empty);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId);
        }

        return totalBets;
    }
}