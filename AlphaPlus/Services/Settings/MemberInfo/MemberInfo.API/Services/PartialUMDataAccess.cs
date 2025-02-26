using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Models;
using AlphaPlus.Settings.MemberInfo.API.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    public class PartialUMDataAccess(IDynamicRepository repository) : IPartialUMDataAccess
{
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<UMEntity>> GetUMEntities(string siteName)
        => repository.FetchAsync<UMEntity>(new GetUMInfoListCriteria(siteName));
}
}