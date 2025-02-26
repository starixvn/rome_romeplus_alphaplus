using AlphaPlus.PartialUM.Services;
using AlphaPlus.Results.Features.Shared;
using Fanex.Data.Repository;
using Fanex.Result.Core.Sportsbook.DTO;

namespace AlphaPlus.Results.Features.Normal;

public interface ISportsbookService
{
    Task<IEnumerable<Sportsbook>> GetResults(SportsbookParam param);

    Task<Sportsbook> GetResultDetails(SportsbookDetailsParam param);

    Task<Sportsbook> GetBetBuilder(int matchId, int marketId);
}

public class SportsbookService(
    IDynamicRepository repository,
    IResultsService resultsService,
    IPartialUMService partialUMService) : ISportsbookService
{
    public async Task<IEnumerable<Sportsbook>> GetResults(SportsbookParam param)
    {
        var moveBetsDay = await resultsService.GetMoveBetsDate();
        var criteria = new GetSportsbookResultCriteria(
            param.FromDate, param.SportType, param.LeagueId, param.IsShowAll, moveBetsDay);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var (sportsbooks, gameDetails) = await repository.FetchMultipleAsync<Sportsbook, GameDetail>(criteria);
        foreach (var sb in sportsbooks)
        {
            SetDefaultSportsbookDTO(sb);
            sb.GameDetails = gameDetails.Where(i => i.MatchId == sb.MatchId).ToList();
        }

        return sportsbooks;
    }

    public Task<Sportsbook> GetResultDetails(SportsbookDetailsParam param)
        => param.BetType == BetTypeConstants.BetBuilder
        ? GetBetBuilder(param.MatchId, (int)param.BetId)
        : GetScoreInformation(param);

    public async Task<Sportsbook> GetBetBuilder(int matchId, int marketId)
    {
        var criteria = new GetBetBuidlerResultDetailsCriteria(matchId, marketId);
        var (sportsbooks, gameDetails) = await repository.FetchMultipleAsync<Sportsbook, BetBuilderDetail>(criteria);
        var sb = sportsbooks.FirstOrDefault();

        if (sb != null)
        {
            SetDefaultSportsbookDTO(sb);
            sb.BetBuilderDetails = gameDetails.ToList();
        }

        return sb;
    }

    private async Task<Sportsbook> GetScoreInformation(SportsbookDetailsParam param)
    {
        var criteria = param.IsViewResultFromBetList
            ? new GetSportsbookResultBetListCriteria(param.MatchId, param.BetType, param.BetId, param.Transdesc)
            : new GetSportsbookResultDetailsCriteria(param.MatchId);
        var (sportsbooks, gameDetails) = await repository.FetchMultipleAsync<Sportsbook, GameDetail>(criteria);
        var sb = sportsbooks.FirstOrDefault();

        if (sb != null)
        {
            sb.BetStatus = sb.BetStatus ?? string.Empty;
            sb.EventStatus = sb.EventStatus ?? string.Empty;
            sb.Color = string.Empty;
            sb.MatchCode = sb.MatchCode ?? string.Empty;
            sb.GoalRecord = sb.GoalRecord ?? string.Empty;
            sb.RefNo = string.Empty;

            sb.MatchId = param.MatchId;
            sb.BetID = param.BetId.ToString();
            sb.Bettype = param.BetType;
            sb.Handicap2 = param.Hdp2;
            sb.GameDetails = gameDetails.Select(gd =>
            {
                gd.MatchId = param.MatchId;
                return gd;
            }).ToList();
        }

        return sb;
    }

    private static void SetDefaultSportsbookDTO(Sportsbook sb)
    {
        sb.BetStatus = sb.BetStatus ?? string.Empty;
        sb.EventStatus = sb.EventStatus ?? string.Empty;
        sb.Color = sb.Color ?? string.Empty;
        sb.MatchCode = sb.MatchCode ?? string.Empty;
        sb.GoalRecord = sb.GoalRecord ?? string.Empty;
        sb.RefNo = string.Empty;
    }
}