using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface ISoccerService
{
    Task<IEnumerable<League>> GetLeagues(bool isLive, bool isShowAll, string language);

    Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language);

    Task<IEnumerable<SoccerResult>> GetResults(int custId, string matchIds, bool isLive, string language);
}

public class SoccerService : ISoccerService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public SoccerService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public async Task<IEnumerable<League>> GetLeagues(bool isLive, bool isShowAll, string language)
    {
        var leagues = await repository.FetchAsync<League>(new GetSoccerLeagueCriteria(isLive, isShowAll));
        foreach (var item in leagues)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
        }

        return leagues;
    }

    public async Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language)
    {
        var matches = await repository.FetchAsync<Match>(new GetSoccerMatchCriteria(leagueId, isLive));
        foreach (var item in matches)
        {
            item.HomeName = cacheRefService.GetTeamName(item.HomeId, language);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId, language);
        }

        return matches;
    }

    public async Task<IEnumerable<SoccerResult>> GetResults(int custId, string matchIds, bool isLive, string language)
    {
        var (matches, betTypes, winloss) = await repository.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(
            new GetSoccerForecastCriteria(custId, matchIds, isLive));

        if (!matches.Any())
        {
            return Enumerable.Empty<SoccerResult>();
        }

        var results = new List<SoccerResult>();
        var betTypeLookup = betTypes.ToLookup(b => b.MatchId);
        var winlossLookup = winloss.ToLookup(wl => wl.MatchId);

        foreach (var match in matches)
        {
            match.HomeName = cacheRefService.GetTeamName(match.HomeId, language);
            match.AwayName = cacheRefService.GetTeamName(match.AwayId, language);

            results.Add(BuildResult(match, betTypeLookup[match.MatchId], winlossLookup[match.MatchId], language));
        }

        return results;
    }

    private SoccerResult BuildResult(
        SoccerMatch match,
        IEnumerable<SoccerBetType> betTypes,
        IEnumerable<SoccerWinLoss> winloss,
        string language)
    {
        const int HalfTimeMinutes = 45;
        if (match.IsLive
            && match.KickoffTime.AddMinutes(HalfTimeMinutes) < DateTime.Now)
        {
            betTypes = betTypes.Where(b => IsFullTimeBetType(b.BetTypeId));
        }

        var betTypeForecasts = new List<SoccerBetTypeForecast>();
        SoccerBetTypeForecast handicapBetType = null;
        var totalBetCount = 0;
        var totalTurnover = 0M;
        var totalForecastWinLoss = 0M;
        var winlossHash = winloss.ToDictionary(kvp => kvp.BetTypeId, kvp => kvp);

        foreach (var item in betTypes)
        {
            if (winlossHash.TryGetValue(item.BetTypeId, out var winlossName))
            {
                var betTypeForecast = BuildBetTypeForecast(item, winlossName, match, handicapBetType, language);

                if (betTypeForecast.IsHandicap)
                {
                    handicapBetType = betTypeForecast;
                }

                totalBetCount += betTypeForecast.BetCount;
                totalTurnover += betTypeForecast.Turnover;

                if (match.IsLive)
                {
                    totalForecastWinLoss += betTypeForecast.WinLossValues.ElementAt(betTypeForecast.MainWinLossIndex);
                }

                betTypeForecasts.Add(betTypeForecast);
            }
        }

        return new SoccerResult
        {
            BetTypeForecasts = betTypeForecasts.OrderBy(b => b.BetType),
            TotalBetCount = totalBetCount,
            TotalTurnover = totalTurnover,
            TotalForecastWinLoss = totalForecastWinLoss,
            MatchId = match.MatchId,
            HomeId = match.HomeId,
            HomeName = match.HomeName,
            AwayId = match.AwayId,
            AwayName = match.AwayName,
            ShowTime = BuildShowTime(match.ShowTime),
            IsLive = match.IsLive,
            LiveHomeScore = match.LiveHomeScore,
            LiveAwayScore = match.LiveAwayScore,
            HasLive = match.HasLive,
            KickoffTime = match.KickoffTime
        };
    }

    private static bool IsFullTimeBetType(int betType)
        => betType == BetTypeConstants.Handicap
        || betType == BetTypeConstants.OverUnder
        || betType == BetTypeConstants._1X2;

    private static string BuildShowTime(string showTime)
    {
        if (!string.IsNullOrWhiteSpace(showTime))
        {
            var showTimeStrings = showTime.Trim().Split(' ');
            var totalShowTimeStrings = showTimeStrings.Count();

            return totalShowTimeStrings > 2
                ? $"{string.Join(" ", showTimeStrings.Take(totalShowTimeStrings - 1))}<br/>{showTimeStrings.Last()}"
                : string.Join("<br/>", showTimeStrings);
        }

        return string.Empty;
    }

    private SoccerBetTypeForecast BuildBetTypeForecast(
        SoccerBetType betType,
        SoccerWinLoss winloss,
        SoccerMatch match,
        SoccerBetTypeForecast handicapBetType,
        string language)
    {
        var winlossColumnNames = new List<string>();
        var winlossValues = new List<decimal>();
        var betTypeForecast = new SoccerBetTypeForecast
        {
            BetType = betType.BetTypeId,
            BetTypeName = cacheRefService.GetBetTypeName(betType.BetTypeId, language),
            BetCount = winloss.BetCount,
            Turnover = winloss.Turnover,
            IsHandicap = betType.BetTypeId == BetTypeConstants.Handicap
                || betType.BetTypeId == BetTypeConstants._1stHandicap,
            IsOverUnder = betType.BetTypeId == BetTypeConstants.OverUnder
                || betType.BetTypeId == BetTypeConstants._1stOverUnder,
            Is1X2 = betType.BetTypeId == BetTypeConstants._1X2
                || betType.BetTypeId == BetTypeConstants._1st1X2,
            WinLossColumnNames = winlossColumnNames,
            WinLossValues = winlossValues
        };
        AddWinLossNameAndValue(betTypeForecast.Is1X2, winlossColumnNames, winlossValues, betType, winloss);

        var i = 0;
        var isMainScore = false;
        var winlossColumnNamesCount = winlossColumnNames.Count;
        while (!isMainScore && i < winlossColumnNamesCount)
        {
            isMainScore = IsMainScore(
                betTypeForecast,
                match,
                i,
                handicapBetType,
                winloss.HighlightValue_3_8,
                winlossColumnNames[i]);
            if (isMainScore)
            {
                betTypeForecast.MainWinLossIndex = i;
            }
            i++;
        }

        return betTypeForecast;
    }

    private static void AddWinLossNameAndValue(
        bool is1X2,
        List<string> winLossColumnNames,
        List<decimal> winLossValues,
        SoccerBetType betType,
        SoccerWinLoss winloss)
    {
        if (is1X2)
        {
            winLossValues.Add(betType.WinLoss1);
            winLossValues.Add(betType.WinLoss2);
            winLossValues.Add(betType.WinLoss3);

            winLossColumnNames.Add("1");
            winLossColumnNames.Add("X");
            winLossColumnNames.Add("2");
        }
        else
        {
            winLossValues.Add(betType.WinLoss1);
            winLossValues.Add(betType.WinLoss2);
            winLossValues.Add(betType.WinLoss3);
            winLossValues.Add(betType.WinLoss4);
            winLossValues.Add(betType.WinLoss5);

            winLossColumnNames.Add(winloss.Column1.ToString());
            winLossColumnNames.Add(winloss.Column2.ToString());
            winLossColumnNames.Add(winloss.Column3.ToString());
            winLossColumnNames.Add(winloss.Column4.ToString());
            winLossColumnNames.Add(winloss.Column5.ToString());
        }
    }

    private static bool IsMainScore(
        SoccerBetTypeForecast betTypeForecast,
        SoccerMatch match,
        int winlossIndex,
        SoccerBetTypeForecast handicapBetType,
        int highlightValueOverUnder,
        string currentWinLossName)
    {
        if (betTypeForecast.IsHandicap && winlossIndex == 2)
        {
            return true;
        }

        return IsMainScoreOverUnder(betTypeForecast, match, winlossIndex, highlightValueOverUnder, currentWinLossName)
            ? true
            : IsMainScore1X2(betTypeForecast, match, winlossIndex, handicapBetType);
    }

    private static bool IsMainScoreOverUnder(
        SoccerBetTypeForecast betTypeForecast,
        SoccerMatch match,
        int winlossIndex,
        int highlightValueOverUnder,
        string currentWinLossName)
    {
        if (betTypeForecast.IsOverUnder && match.IsLive && winlossIndex == 4)
        {
            return true;
        }

        return betTypeForecast.IsOverUnder
            && !match.IsLive
            && currentWinLossName.Equals(highlightValueOverUnder.ToString());
    }

    private static bool IsMainScore1X2(
        SoccerBetTypeForecast betTypeForecast,
        SoccerMatch match,
        int winlossIndex,
        SoccerBetTypeForecast handicapBetType)
    {
        if (betTypeForecast.Is1X2)
        {
            if (match.IsLive)
            {
                return IsMainScore1X2AndIsLive(match, winlossIndex);
            }

            if (!match.IsLive && handicapBetType != null)
            {
                var handicapValue = Convert.ToInt32(handicapBetType.WinLossColumnNames.ElementAt(2));
                var indexIs0 = handicapValue > 0 && winlossIndex == 0;
                var indexIs1 = handicapValue == 0 && winlossIndex == 1;
                var indexIs2 = handicapValue < 0 && winlossIndex == 2;

                return indexIs0 || indexIs1 || indexIs2;
            }
        }

        return false;
    }

    private static bool IsMainScore1X2AndIsLive(SoccerMatch matchForecast, int winlossIndex)
    {
        var indexIs0 = matchForecast.LiveHomeScore > matchForecast.LiveAwayScore && winlossIndex == 0;
        var indexIs1 = matchForecast.LiveHomeScore == matchForecast.LiveAwayScore && winlossIndex == 1;
        var indexIs2 = matchForecast.LiveHomeScore < matchForecast.LiveAwayScore && winlossIndex == 2;

        return indexIs1 || indexIs0 || indexIs2;
    }
}