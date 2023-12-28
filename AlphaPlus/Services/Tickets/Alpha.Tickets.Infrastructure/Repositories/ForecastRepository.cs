using Alpha.Tickets.Infrastructure.Criteria.Forecast;
using Alpha.Tickets.Infrastructure.Extensions;
using Alpha.Tickets.Infrastructure.Models.Forecast;
using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Repositories;

public interface IForecastRepository
{
    Task<IEnumerable<int>> GetBetTypeIdsSupportScoreMap();
}

public class ForecastRepository : IForecastRepository
{
    private readonly IDynamicRepository repository;

    public ForecastRepository(IDynamicRepository repository)
        => this.repository = repository;

    public async Task<IEnumerable<int>> GetBetTypeIdsSupportScoreMap()
    {
        var scoreMaps = await repository.FetchAsync<ScoreMap>(new GetScoreMapBetTypesCriteria());

        return scoreMaps.Select(s => s.FirstId.ToListInt()).SelectMany(x => x).Distinct();
    }
}