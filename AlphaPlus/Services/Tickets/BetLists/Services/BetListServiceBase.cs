using Alpha.IPInfo.Services;
using AlphaPlus.BetLists.Models;
using AlphaPlus.Utilities.IP;
using Fanex.BetList.Core.Entities;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Services;

public class BetListServiceBase
{
    protected readonly IDynamicRepository repository;
    protected readonly IIPInfoService ipInfoService;
    protected readonly IPService ipService;

    protected BetListServiceBase(
        IDynamicRepository repository,
        IIPInfoService ipInfoService,
        IPService ipService)
        => (this.repository, this.ipInfoService, this.ipService) = (repository, ipInfoService, ipService);


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