using AlphaPlus.Results.Criteria;
using AlphaPlus.Results.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Results.Services;

public interface INormalResultService
{
    Task<IEnumerable<League>> GetLeagues(DateTime resultDate, int sportId, DateTime moveBetsDate, bool localTest);

    Task<IEnumerable<Sport>> GetSports(DateTime resultDate, IDictionary<int, bool> sportsStatus, DateTime moveBetsDate, bool localTest);
}

public class NormalResultService : INormalResultService
{
    private readonly IDynamicRepository repository;
    private readonly IResultServiceFactory resultServiceFactory;

    public NormalResultService(IDynamicRepository repository, IResultServiceFactory resultServiceFactory)
        => (this.repository, this.resultServiceFactory) = (repository, resultServiceFactory);

    public Task<IEnumerable<League>> GetLeagues(DateTime resultDate, int sportId, DateTime moveBetsDate, bool localTest)
    {
        var criteria = new GetResultLeaguesByDateCriteria(moveBetsDate, localTest)
        {
            EventDate = resultDate.ToShortDateString(),
            SportId = sportId
        };

        return repository.FetchAsync<League>(criteria);
    }

    public async Task<IEnumerable<Sport>> GetSports(DateTime resultDate, IDictionary<int, bool> sportsStatus, DateTime moveBetsDate, bool localTest)
    {
        var criteria = new GetResultSportsByDateCriteria(moveBetsDate, localTest)
        {
            EventDate = resultDate.ToShortDateString()
        };

        var sports = (await repository.FetchAsync<Sport>(criteria)).ToList();

        if (sports.Any())
        {
            sports.RemoveAll(sport => sportsStatus.TryGetValue(sport.Id, out var status) && !status);
        }

        return sports;
    }

    public string GetResults(NormalResultQuery request)
    {
        var resultService = resultServiceFactory.CreateProductResultService(request.SportId, request.BetTypeId);

        return resultService.GetResult(request);
    }
}
