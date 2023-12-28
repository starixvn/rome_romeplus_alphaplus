using Alpha.Tickets.Infrastructure.Criteria.IPRange;
using Alpha.Tickets.Infrastructure.Models.IPRange;
using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Repositories;

public interface IIPRangeRepository
{
    Task<IEnumerable<IPRangeV4>> GetIPRangeV4(string ipList);

    Task<IEnumerable<IPRangeV4>> GetIPRangeV4ByCountries(string countries);

    Task<IEnumerable<IPRangeV6>> GetIPRangeV6(string ipList);
}

public class IPRangeRepository : IIPRangeRepository
{
    private readonly IDynamicRepository repository;

    public IPRangeRepository(IDynamicRepository repository)
        => this.repository = repository;

    public Task<IEnumerable<IPRangeV4>> GetIPRangeV4(string ipList)
    => repository.FetchAsync<IPRangeV4>(new GetIPRangeV4Criteria(ipList));

    public Task<IEnumerable<IPRangeV4>> GetIPRangeV4ByCountries(string countries)
        => repository.FetchAsync<IPRangeV4>(new GetIPRangeV4ByCountryCriteria(countries));

    public Task<IEnumerable<IPRangeV6>> GetIPRangeV6(string ipList)
        => repository.FetchAsync<IPRangeV6>(new GetIPRangeV6Criteria(ipList));
}
