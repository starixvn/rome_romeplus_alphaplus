using AlphaPlus.PartialUM.DataAccess;
using AlphaPlus.PartialUM.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Shared;

internal class GetUMInfoListCriteria(string siteName) : CriteriaBase
{
    public string SiteName { get; } = siteName;

    public override string GetSettingKey() => "Acc_CheckUM_ByReport";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(SiteName);
}

public class PartialUMDataAccess(IDynamicRepository repository) : IPartialUMDataAccess
{
    private readonly IDynamicRepository repository = repository;

    public Task<IEnumerable<UMEntity>> GetUMEntities(string siteName)
        => repository.FetchAsync<UMEntity>(new GetUMInfoListCriteria(siteName));
}
