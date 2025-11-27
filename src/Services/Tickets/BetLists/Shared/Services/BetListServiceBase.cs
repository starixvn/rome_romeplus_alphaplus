using AlphaPlus.IPInfo.Services;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.Shared.Services;

public class BetListServiceBase(
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService)
{
    protected readonly IDynamicRepository repository = repository;
    protected readonly IIPInfoService ipInfoService = ipInfoService;
    protected readonly IPService ipService = ipService;

    protected async Task<BetListData> GetBetList(ICriteria criteria, bool displayVNIP, int superId, string system)
    {
        var (tickets, ticketData) = await repository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        var shouldHideIP = await ipService.CheckHideIP(superId, system);
        
        AssignIP(tickets, shouldHideIP);
        await AssignCountry(tickets, displayVNIP, shouldHideIP);

        return new BetListData(tickets, ticketData);
    }

    protected void AssignIP(IEnumerable<TicketDto> tickets, bool shouldHideIP)
    {
        if (shouldHideIP)
        {
            foreach (var ticket in tickets)
            {
                if (!string.IsNullOrWhiteSpace(ticket.IP))
                {
                    ticket.IP = string.Empty;
                }
            }
        }
    }

    protected async Task AssignCountry(IEnumerable<TicketDto> tickets, bool displayVNIP, bool shouldHideIP)
    {
        if (!tickets.Any())
        {
            return;
        }

        if (shouldHideIP)
        {
            foreach (var ticket in tickets)
            {
                ticket.Country = string.Empty;
            }
            return;
        }

        var ipList = tickets
            .Where(t => !string.IsNullOrWhiteSpace(t.IP) && (displayVNIP || !ipService.IsVNIP(t.IP.Trim())))
            .Select(t => t.IP.Trim())
            .Distinct();
        var countryDictionary = await GetCountries(ipList);

        string country;
        foreach (var item in tickets)
        {
            if (!string.IsNullOrWhiteSpace(item.IP)
                && countryDictionary.TryGetValue(item.IP.Trim(), out country))
            {
                item.Country = country;
            }
        }
    }

    protected async Task<IDictionary<string, string>> GetCountries(IEnumerable<string> ipList)
    {
        try
        {
            return await ipInfoService.GetCountries(ipList);
        }
        catch (Exception ex)
        {
            await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
        }

        return new Dictionary<string, string>();
    }

    protected async Task UpdateOddsMixParlay(IEnumerable<TicketDto> tickets)
    {
        var transIds = GetMixParlayMissingOddsTransIds(tickets);

        if (!string.IsNullOrWhiteSpace(transIds))
        {
            var oddsMixParlay = await GetOddsMixParlay(transIds);

            foreach (var ticket in tickets)
            {
                if (oddsMixParlay.TryGetValue(ticket.TransId, out var oddsItem))
                {
                    ticket.Odds = oddsItem.Odds;
                    ticket.MasterOdds = oddsItem.MasterOdds;
                }
            }
        }
    }

    private static string GetMixParlayMissingOddsTransIds(IEnumerable<TicketDto> tickets)
    {
        var ticketsMissingOdds = tickets.Where(i => i.BetTypeId == BetTypeConstants.CombineMixParlay && i.BetTeam == "1" && i.OddsSpread != 3 && i.Odds == 0);

        return string.Join(',', ticketsMissingOdds.Select(i => i.TransId));
    }

    private async Task<IDictionary<long, OddsMixParlayModel>> GetOddsMixParlay(string transIds)
    {
        var oddsMixParlay = new Dictionary<long, OddsMixParlayModel>();
        var rawdata = await repository.FetchAsync<OddsMixParlayModel>(new GetOddsOfMixParlayCriteria(transIds));

        foreach (var item in rawdata)
        {
            oddsMixParlay[item.TransId] = item;
        }

        return oddsMixParlay;
    }
}