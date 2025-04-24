using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Models;
using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Shared;

public class PartialUMDataAccess(IDynamicRepository repository) : IPartialUMDataAccess
{
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<UMEntity>> GetUMEntities(string siteName)
        => repository.FetchAsync<UMEntity>(new GetUMInfoListCriteria(siteName));
}