using Fanex.Data.Repository;
using Fanex.Result.Core.MetricGaming.DTO;

namespace AlphaPlus.Results.Features.Normal;

public interface IMetricGamingService
{
    Task<IEnumerable<MetricGaming>> GetResults(long betId);
}

public class MetricGamingService(IDynamicRepository repository) : IMetricGamingService
{
    public Task<IEnumerable<MetricGaming>> GetResults(long betId)
        => repository.FetchAsync<MetricGaming>(new GetMetricGamingResultCriteria(betId));
}
