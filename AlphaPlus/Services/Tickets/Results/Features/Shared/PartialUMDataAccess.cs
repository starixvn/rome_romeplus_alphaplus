using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Shared;

public class PartialUMDataAccess(IDynamicRepository repository) : IPartialUMDataAccess
{
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<UMEntity>> GetUMEntities(string siteName)
        => repository.FetchAsync<UMEntity>(new GetUMInfoListCriteria(siteName));
}