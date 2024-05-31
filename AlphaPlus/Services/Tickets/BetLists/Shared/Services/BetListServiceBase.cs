using AlphaPlus.IPInfo.Services;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;
using AlphaPlus.BetLists.Shared.Models;

namespace AlphaPlus.BetLists.Shared.Services;

public class BetListServiceBase(
    IDynamicRepository repository,
    IIPInfoService ipInfoService,
    IPService ipService)
{
    protected readonly IDynamicRepository repository = repository;
    protected readonly IIPInfoService ipInfoService = ipInfoService;
    protected readonly IPService ipService = ipService;

    protected async Task<BetListData> GetBetList(ICriteria criteria, bool displayVNIP)
    {
        var (tickets, ticketData) = await repository.FetchMultipleAsync<TicketDto, TicketData>(criteria);
        await AssignCountry(tickets, displayVNIP);

        return new BetListData(tickets, ticketData);
    }

    protected async Task AssignCountry(IEnumerable<ITicket> tickets, bool displayVNIP)
    {
        if (tickets.Any())
        {
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
                    ((TicketDto)item).Country = country;
                }
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
            //ExceptionHandler.Handle(ex);
        }

        return new Dictionary<string, string>();
    }
}