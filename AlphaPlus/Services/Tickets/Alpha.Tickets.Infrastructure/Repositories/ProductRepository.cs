using Alpha.Tickets.Infrastructure.Criteria;
using Alpha.Tickets.Infrastructure.Models;
using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts(string site, int currency, byte settingStatus, bool isHidden = false);
}

public class ProductRepository : IProductRepository
{
    private readonly IDynamicRepository repository;

    public ProductRepository(IDynamicRepository repository)
        => this.repository = repository;

    public async Task<IEnumerable<Product>> GetProducts(string site, int currency, byte settingStatus, bool isHidden = false)
        => (await repository.FetchAsync<Product>(new GetProductsCriteria(site, currency, settingStatus))).Where(p => p.IsHidden == isHidden);
}
