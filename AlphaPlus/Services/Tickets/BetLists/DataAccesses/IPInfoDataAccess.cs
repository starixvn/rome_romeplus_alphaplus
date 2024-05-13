using AlphaPlus.IPInfo.DataAccess;
using AlphaPlus.IPInfo.Models;
using AlphaPlus.BetLists.Criteria.IPInfo;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.DataAccesses;

public class IPInfoDataAccess : IIPInfoDataAccess
{
    private readonly IDynamicRepository repository;

    public IPInfoDataAccess(IDynamicRepository repository)
        => this.repository = repository;

    public Task<IEnumerable<IPV4Information>> GetIPV4Info(string ipList)
        => repository.FetchAsync<IPV4Information>(new GetIPV4InfoCriteria(ipList));

    public Task<IEnumerable<IPV4Information>> GetIPV4InfoByTopCountries(string countries)
        => repository.FetchAsync<IPV4Information>(new GetIPV4InfoByTopCountryCriteria(countries));

    public Task<IEnumerable<IPV6Information>> GetIPV6Info(string ipList)
        => repository.FetchAsync<IPV6Information>(new GetIPV6InfoCriteria(ipList));
}
