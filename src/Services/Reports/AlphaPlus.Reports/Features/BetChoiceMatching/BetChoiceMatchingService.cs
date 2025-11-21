using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public interface IBetChoiceMatchingService
{
    Task<(IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>)> GetBetChoiceMatching(BetChoiceMatchingParam param);

    Task<IEnumerable<BetChoiceMatchingEventModel>> GetBetChoiceMatchingExcel(BetChoiceMatchingExcelParam param);

    Task<BetChoiceMatchingUplinesModel> QueryUplines(int custId, string siteName);
}

public class BetChoiceMatchingService(
    IDynamicRepository repository,
    IReportDateService reportDateService,
    IPService ipService,
    ICacheRefService cacheRefService,
    IPartialUMService partialUMService) : IBetChoiceMatchingService
{
    private const int Handicap = 1;
    private const int OverUnder = 3;
    private const int FirstHalfHandicap = 7;
    private const int FirstHalfOverUnder = 8;
    private static readonly HashSet<int> OddsBetTypes = new HashSet<int>([Handicap, OverUnder, FirstHalfHandicap, FirstHalfOverUnder]);

    private const byte OddsTypesDecimal = 1;
    private const byte OddsTypesUS = 5;
    private const int USOddsTypeMultiply = 100;
    private const string OddsFormat = "{0:#,##0.###}";

    private readonly IDynamicRepository repository = repository;
    private readonly IReportDateService reportDateService = reportDateService;
    private readonly IPService ipService = ipService;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly IPartialUMService partialUMService = partialUMService;

    public async Task<(IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>)> GetBetChoiceMatching(BetChoiceMatchingParam param)
    {
        var moveBetsDay = await reportDateService.GetMoveBetsDay();
        var criteria = new GetBetChoiceMatchingCriteria(
            param.CustId,
            param.WinLossDate,
            moveBetsDay,
            param.BetCount,
            param.IsExportExcel);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var (tickets, ips) = await repository.FetchMultipleAsync<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(criteria);

        return await BuildBetChoiceMatching(param.IsAdminCanViewVNIP, tickets, ips, param.SuperId, param.System);
    }

    private async Task<(IEnumerable<BetChoiceMatchingEventModel>, IEnumerable<BetChoiceMatchingSharedIPModel>)> BuildBetChoiceMatching(
        bool isAdminCanViewVNIP,
        IEnumerable<BetChoiceMatchingTicketModel> tickets,
        IEnumerable<BetChoiceMatchingSharedIPModel> ips,
        int superId,
        string system)
    {
        var vnIPs = new HashSet<string>();
        foreach (var ticket in tickets)
        {
            CalculateDisplayOdds(ticket);
            if (!isAdminCanViewVNIP && !string.IsNullOrWhiteSpace(ticket.IP))
            {
                if (vnIPs.Contains(ticket.IP))
                {
                    ticket.IP = "--";
                }
                else if (ipService.IsVNIP(ticket.IP))
                {
                    ticket.IP = "--";
                    vnIPs.Add(ticket.IP);
                }
            }

            if(await ipService.CheckHideIP(superId, system))
            {
                ticket.IP = "--";
            }
        }

        var events = tickets.GroupBy(
            ticket => ticket.EventKey,
            (eventKey, eventTickets) =>
            {
                var users = eventTickets.GroupBy(
                    ticket => ticket.CustId,
                    (_, userTickets) => new BetChoiceMatchingUserModel(userTickets));

                return new BetChoiceMatchingEventModel(users);
            });

        if (!isAdminCanViewVNIP)
        {
            ips = ips.Where(ip => !vnIPs.Contains(ip.IP));
        }

        return (events, ips);
    }

    private void CalculateDisplayOdds(BetChoiceMatchingTicketModel ticket)
    {
        var isShowOddsType = OddsBetTypes.Contains(ticket.BetType) && ticket.OddsType > 0;
        if (isShowOddsType)
        {
            if (ticket.OddsType == OddsTypesDecimal)
            {
                ticket.Odds += 1;
            }
            else if (ticket.OddsType == OddsTypesUS)
            {
                ticket.Odds *= USOddsTypeMultiply;
            }

            ticket.DisplayOddsType = cacheRefService.GetOddsTypeName(ticket.OddsType.ToString());
        }

        ticket.DisplayOdds = string.Format(OddsFormat, ticket.Odds);
    }

    public async Task<IEnumerable<BetChoiceMatchingEventModel>> GetBetChoiceMatchingExcel(BetChoiceMatchingExcelParam param)
    {
        var (events, _) = await GetBetChoiceMatching(param);
        var filteredEvents = new List<BetChoiceMatchingEventModel>();

        foreach (var eventModel in events)
        {
            var filteredUsers = eventModel.Users;
            if (!string.IsNullOrEmpty(param.Username))
            {
                var usernames = new HashSet<string>(param.Username.Split(',').Select(x => x.ToLowerInvariant()));
                filteredUsers = filteredUsers.Where(user => usernames.Contains(user.Username.ToLowerInvariant()));
            }

            if (param.Stake.HasValue)
            {
                filteredUsers = filteredUsers.Where(user => user.Stake >= param.Stake.Value);
            }

            if (param.MemberWinLossValue.HasValue)
            {
                switch (param.MemberWinLossOperator)
                {
                    case MemberWinLossOperators.GreatorOrEqual:
                        filteredUsers = filteredUsers.Where(user => !user.IsRunning && user.MemberWinLoss >= param.MemberWinLossValue.Value);
                        break;

                    case MemberWinLossOperators.Equal:
                        filteredUsers = filteredUsers.Where(user => !user.IsRunning && user.MemberWinLoss == param.MemberWinLossValue.Value);
                        break;

                    default:
                        filteredUsers = filteredUsers.Where(user => !user.IsRunning && user.MemberWinLoss <= param.MemberWinLossValue.Value);
                        break;
                }
            }

            var filteredUsersList = filteredUsers.ToList();
            if (filteredUsersList.Count > 0)
            {
                filteredEvents.Add(new BetChoiceMatchingEventModel(filteredUsersList));
            }
        }

        return filteredEvents;
    }

    public async Task<BetChoiceMatchingUplinesModel> QueryUplines(int custId, string siteName)
    {
        var criteria = new BetChoiceUplinesCriteria(custId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

        return await repository.GetAsync<BetChoiceMatchingUplinesModel>(criteria);
    }
}