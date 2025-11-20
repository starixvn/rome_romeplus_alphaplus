using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.IPInfo.Services;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using AlphaPlus.Core.Models;

namespace AlphaPlus.BetLists.LastBets;

public interface ILastBetsService
{
    Task<IEnumerable<TicketDto>> GetBetList(GetLastBetsParam param);

    Task<IEnumerable<TicketDto>> GetBetListNew(GetLastBetsNewParam param);
}

public class LastBetsService(
    IPartialUMService partialUMService,
    IMemoryCache cacheService,
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService) : BetListServiceBase(repository, ipInfoService, ipService), ILastBetsService
{
    private const string RunningMatchesCacheKey = "AlphaPlus.BetLists.LastBets.RunningMatches";
    private const string RunningOutrightMatchesCacheKey = "AlphaPlus.BetLists.LastBets.RunningOutrightMatches";
    private const string RunningOtherMatchesCacheKey = "AlphaPlus.BetLists.LastBets.RunningOtherMatches";

    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly IMemoryCache cacheService = cacheService;

    public Task<IEnumerable<TicketDto>> GetBetList(GetLastBetsParam param)
    {
        var criteria = new GetLastBetsCriteria(param.BetType, param.CustId, param.TransId, param.PageSize);

        return GetBetList(criteria, param.DisplayVNIP, param.SuperId, param.System, param.SiteName, param.RunningMatchCacheTime);
    }

    public Task<IEnumerable<TicketDto>> GetBetListNew(GetLastBetsNewParam param)
    {
        var userLevel = Enumeration.FromValue<UserLevel>(param.UserLevel);
        var criteria = new GetLastBetsNewCriteria(
            param.CustId,
            param.TransId,
            param.MemberList,
            param.SportTypeList,
            param.LeagueList,
            param.BetTypeList,
            param.LiveIndicator,
            param.MinStake,
            param.LimitRowCount,
            userLevel.IsSuperLevel(),
            userLevel.IsMasterLevel());

        return GetBetList(criteria, param.DisplayVNIP, param.SuperId, param.System, param.SiteName, param.RunningMatchCacheTime);
    }

    private async Task<IEnumerable<TicketDto>> GetBetList(
        ICriteria criteria,
        bool displayVNIP,
        int superId,
        string system,
        string siteName,
        int cacheTimeInMinutes)
    {
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);
        var tickets = await repository.FetchAsync<TicketDto>(criteria);

        if (tickets.Any())
        {
            var outrightMatchesTask = GetRunningOutrightMatches(cacheTimeInMinutes);
            var otherMatchesTask = GetRunningOtherMatches(cacheTimeInMinutes);
            await Task.WhenAll(outrightMatchesTask, otherMatchesTask);

            var outrightMatches = await outrightMatchesTask;
            var otherMatches = await otherMatchesTask;

            foreach (var ticket in tickets)
            {
                var matches = BetTypeConstants.IsOutright(ticket.BetTypeId) ? outrightMatches : otherMatches;
                if (!matches.TryGetValue(ticket.MatchId, out var matchInfo))
                {
                    matchInfo = await GetMatchInfo(ticket.MatchId, cacheTimeInMinutes);
                }

                AssignMatchInfoToTicket(ticket, matchInfo);
            }

            await AssignCountry(tickets, displayVNIP);
            await HideIP(tickets, superId, system);
            await UpdateOddsMixParlay(tickets);
        }

        return tickets;
    }

    private async Task<IDictionary<int, MatchInfo>> GetRunningOutrightMatches(int cacheTimeInMinutes)
        => await cacheService.GetOrCreateAsync(
        RunningOutrightMatchesCacheKey,
        async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTimeInMinutes));
            var matches = await repository.FetchAsync<MatchInfo>(new GetRunningOutrightMatchCriteria());
            return BuildHashTableMatches(matches);
        });

    private async Task<IDictionary<int, MatchInfo>> GetRunningOtherMatches(int cacheTimeInMinutes)
        => await cacheService.GetOrCreateAsync(
        RunningOtherMatchesCacheKey,
        async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTimeInMinutes));
            var matches = await repository.FetchAsync<MatchInfo>(new GetRunningOtherMatchCriteria());
            return BuildHashTableMatches(matches);
        });

    private static IDictionary<int, MatchInfo> BuildHashTableMatches(IEnumerable<MatchInfo> matches)
    {
        var matchHashTable = new Dictionary<int, MatchInfo>();

        foreach (var item in matches)
        {
            matchHashTable[item.MatchId] = item;
        }

        return matchHashTable;
    }

    private async Task<MatchInfo> GetMatchInfo(int matchId, int cacheTimeInMinutes)
    {
        var cachedKey = $"{RunningMatchesCacheKey}_{matchId}";
        var matchInfo = cacheService.Get<MatchInfo>(cachedKey);

        if (matchInfo == null && !IsDummyMatch(matchId))
        {
            matchInfo = await repository.GetAsync<MatchInfo>(new GetMatchInfoByIdCriteria(matchId));

            if (matchInfo != null)
            {
                cacheService.Set(
                    cachedKey,
                    matchInfo,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTimeInMinutes)));
            }
        }

        return matchInfo;
    }

    private static bool IsDummyMatch(int matchId) => matchId <= 999;

    private static void AssignMatchInfoToTicket(TicketDto ticket, MatchInfo matchInfo)
    {
        if (matchInfo != null)
        {
            ticket.MatchCode = matchInfo.MatchCode;
            ticket.SportTypeId = matchInfo.SportType;
            ticket.LeagueId = matchInfo.LeagueId;
            ticket.LeagueGroupId = matchInfo.LeagueGroupId;
            ticket.HomeId = matchInfo.HomeId;
            ticket.AwayId = matchInfo.AwayId;
            ticket.Race = matchInfo.Race;
            ticket.ShowTime = matchInfo.ShowTime;
            ticket.IsNeutral = matchInfo.IsNeutral;
            ticket.EventStatus = matchInfo.BetStatus;
            ticket.SettlementStatus = matchInfo.SettlementStatus;
            ticket.EventDate = matchInfo.EventDate;
            ticket.MatchStatus = matchInfo.Status;
        }
    }
}
