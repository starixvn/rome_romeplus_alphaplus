using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using AlphaPlus.Shared.AppResources.Betlist.Services;
using AlphaPlus.BetLists.Shared.Helpers;
using AlphaPlus.BetLists.Shared.Models;
using AlphaPlus.PartialUM.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.LastBets;

public interface IFilterBetsService
{
    Task<IEnumerable<Sport>> GetSports(GetSportsParam param);

    Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId);

    Task<Result> AddFilter(AddFilterParam param);

    Task<Result> UpdateFilter(UpdateFilterParam param);

    Task<Result> DeleteFilter(DeleteFilterParam param);

    Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId, string siteName, AddFilterParam unsavedFilter);

    Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId, string siteName, AddFilterParam unsavedFilter);
}

public class FilterBetsService(
    IPartialUMService partialUMService,
    IDynamicRepository repository) : IFilterBetsService
{
    private const int SoccerSportTypeId = 1;
    private const int BasketballSportTypeId = 2;
    private const int TennisSportTypeId = 5;
    private const int BadmintonSportTypeId = 9;
    private const int ESportsSportTypeId = 43;
    private const string IgnoreSportTypeName = "upcoming";

    private static readonly HashSet<int> LeagueGroupPinGoal = new HashSet<int> { 113, 122, 151 };
    private static readonly HashSet<int> MajorSports = new HashSet<int>
    {
        SoccerSportTypeId,
        BasketballSportTypeId,
        TennisSportTypeId,
        BadmintonSportTypeId,
        ESportsSportTypeId
    };
    private static readonly string[] Languages = ["en-US", "zh-CN", "zh-TW", "th-TH", "vi-VN"];

    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly IDynamicRepository repository = repository;

    public async Task<IEnumerable<Sport>> GetSports(GetSportsParam param)
    {
        var criteria = new GetLeaguesCriteria(param.IsShowAll);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var leagues = await repository.FetchAsync<League>(criteria);
        if (param.IsSiteLabel777)
        {
            leagues = leagues.Where(i => !LeagueGroupPinGoal.Contains(i.LeagueGroupId));
        }

        if (param.SportGroup != SportGroupConstants.All)
        {
            leagues = param.SportGroup == SportGroupConstants.MajorSports
                ? leagues.Where(m => MajorSports.Contains(m.SportType))
                : leagues.Where(m => !MajorSports.Contains(m.SportType));
        }

        return BuildSports(leagues.ToList()).ToList();
    }

    private static IEnumerable<Sport> BuildSports(IEnumerable<League> leagueList)
    {
        var groupBySportTypes = leagueList.GroupBy(l => l.SportType).OrderBy(l => l.Key);

        foreach (var group in groupBySportTypes)
        {
            var sportTypeName = CacheRefServiceHelper.GetSportName(group.Key);
            if (sportTypeName.IndexOf(IgnoreSportTypeName, StringComparison.OrdinalIgnoreCase) == -1)
            {
                yield return new Sport
                {
                    Id = group.Key,
                    Name = sportTypeName,
                    Leagues = BuildLeagues(group.Key, group).OrderBy(l => l.LeagueName).ToList()
                };
            }
        }
    }

    private static IEnumerable<League> BuildLeagues(int sportType, IEnumerable<League> leagueList)
        => leagueList.GroupBy(
        l => l.LeagueId,
        (leagueId, matches) => new League
        {
            SportType = sportType,
            LeagueId = leagueId,
            LeagueGroupId = 0,
            LeagueName = leagueId == 0
                ? Resources.AllLeagues
                : CacheRefServiceHelper.GetLeagueName(leagueId, matches.FirstOrDefault(i => i.LeagueId == leagueId).LeagueGroupId)
        });

    public Task<IEnumerable<MyWatchlistMember>> GetMyWatchlistMembers(int agentId)
        => repository.FetchAsync<MyWatchlistMember>(new GetMonitoredWatchlistMembersCriteria(agentId));

    public async Task<Result> AddFilter(AddFilterParam param)
    {
        if (IsUnsavedFilterName(param.FilterName))
        {
            const int AlreadyExistsErrorCode = 90020;
            return new Result(AlreadyExistsErrorCode, Resources.FilterNameAlreadyExists);
        }

        var criteria = new AddFilterCommand(
            param.CustId,
            param.SubAccId,
            param.FilterName,
            param.IsDefault,
            param.BetType,
            param.Stake,
            BuildSportListXml(param.Sports),
            param.LiveStatus,
            param.FilterByMembers,
            param.InputList,
            param.MyWatchlist,
            param.LanguageKey,
            param.IP);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        await repository.ExecuteAsync(criteria);
        return new Result(
            criteria.ErrCode,
            criteria.ErrCode == 0 ? Resources.AddedSuccessfully : criteria.ErrMsg);
    }

    private static bool IsUnsavedFilterName(string filterName)
    {
        foreach (var name in Languages)
        {
            var resourceValue = Resources.ResourceManager.GetString("UnsavedFilter", new CultureInfo(name));
            if (filterName.Equals(resourceValue, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static string BuildSportListXml(SportXml[] sports)
    {
        var xml = new StringBuilder("<SportList>");

        foreach (var item in sports)
        {
            AppendSportXml(xml, item);
        }

        xml.Append("</SportList>");

        return xml.ToString();
    }

    private static void AppendSportXml(StringBuilder xml, SportXml sport)
        => xml.Append($"<Sport s=\"{sport.Sport}\" l=\"{sport.League}\"/>");

    public async Task<Result> UpdateFilter(UpdateFilterParam param)
    {
        var criteria = new UpdateFilterCommand(
            param.CustId,
            param.SubAccId,
            param.FilterId,
            param.FilterName,
            param.IsDefault,
            param.LanguageKey,
            param.IP);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        await repository.ExecuteAsync(criteria);
        return new Result(
            criteria.ErrCode,
            criteria.ErrCode == 0 ? Resources.UpdateSuccessfully : criteria.ErrMsg);
    }

    public async Task<Result> DeleteFilter(DeleteFilterParam param)
    {
        var criteria = new DeleteFilterCommand(
            param.CustId,
            param.SubAccId,
            param.FilterId,
            param.LanguageKey,
            param.IP);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        await repository.ExecuteAsync(criteria);
        return new Result(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<IEnumerable<FilterConfiguration>> GetFilters(int custId, int subAccId, string siteName, AddFilterParam unsavedFilter)
    {
        var filterConfiguration = (await GetFilterConfiguration(custId, subAccId, siteName))
            .Select(item =>
            {
                item.FilterXml = Deserialize(item.FilterValue);
                item.FilterXml.BetTypeNames = BuildBetTypeNames(item.FilterXml.BetType);
                item.FilterXml.FlatternSports = BuildSports(FlattenSports(item.FilterXml.Sports)).ToList();

                return item;
            })
            .ToList();

        if (unsavedFilter != null)
        {
            filterConfiguration.Insert(0, BuildUnsavedFilterConfiguration(unsavedFilter));
        }

        return filterConfiguration;
    }

    public async Task<IEnumerable<SavedFilter>> GetSavedFilters(int custId, int subAccId, string siteName, AddFilterParam unsavedFilter)
    {
        var filterConfiguration = (await GetFilterConfiguration(custId, subAccId, siteName))
            .Select(f =>
            {
                f.FilterXml = Deserialize(f.FilterValue);
                var (sportTypes, leagues) = BuildSportTypesAndLeagues(f.FilterXml.Sports);

                return new SavedFilter(
                    f.FilterId,
                    f.FilterName,
                    f.IsDefault,
                    f.FilterXml.BetType,
                    f.FilterXml.Stake,
                    f.FilterXml.LiveStatus,
                    sportTypes,
                    leagues,
                    BuildMembers(f.FilterXml.FilterByMembers, f.FilterXml.Members, f.FilterXml.MyWatchlistMembers),
                    f.FilterXml.FilterByMembers);
            }).ToList();

        if (unsavedFilter != null)
        {
            filterConfiguration.Insert(0, BuildUnsavedFilter(unsavedFilter));
        }

        return filterConfiguration;
    }

    private async Task<IEnumerable<FilterConfiguration>> GetFilterConfiguration(int custId, int subAccId, string siteName)
    {
        var criteria = new GetFiltersCriteria(custId, subAccId);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

        return (await repository.FetchAsync<FilterConfiguration>(criteria)).OrderByDescending(f => f.FilterId);
    }

    private static FilterXml Deserialize(string xml)
    {
        var serializer = new XmlSerializer(typeof(FilterXml));

        using (TextReader reader = new StringReader(xml))
        {
            return (FilterXml)serializer.Deserialize(reader);
        }
    }

    private static IEnumerable<string> BuildBetTypeNames(string betTypes)
        => betTypes.Equals("0", StringComparison.OrdinalIgnoreCase)
        ? []
        : betTypes.ToListInt().Select(t => BetTypeHelper.GetBetTypeName(t)).ToList();

    private static IEnumerable<League> FlattenSports(IEnumerable<SportXml> sportXmlList)
    {
        foreach (var item in sportXmlList)
        {
            if (!item.Sport.Equals("0", StringComparison.OrdinalIgnoreCase))
            {
                var sportList = item.Sport.ToListInt();
                foreach (var sport in sportList)
                {
                    var leagueList = item.League.ToListInt();
                    foreach (var league in leagueList)
                    {
                        yield return new League
                        {
                            SportType = sport,
                            LeagueId = league
                        };
                    }
                }
            }
        }
    }

    private static FilterConfiguration BuildUnsavedFilterConfiguration(AddFilterParam param)
    {
        var filterXml = new FilterXml
        {
            BetType = param.BetType,
            Stake = param.Stake,
            Members = param.InputList,
            MyWatchlistMembers = param.MyWatchlist,
            FilterByMembers = param.FilterByMembers,
            LiveStatus = param.LiveStatus,
            Sports = param.Sports.ToList()
        };

        filterXml.BetTypeNames = BuildBetTypeNames(filterXml.BetType);
        filterXml.FlatternSports = BuildSports(FlattenSports(filterXml.Sports)).ToList();

        return new FilterConfiguration
        {
            FilterId = param.FilterId,
            FilterName = Resources.UnsavedFilter,
            FilterXml = filterXml,
            IsDefault = param.IsDefault,
            CreatedDate = DateTime.MinValue,
            WatchList = param.MyWatchlistMembers,
            InputList = param.InputListMembers
        };
    }

    private static (string, string) BuildSportTypesAndLeagues(IEnumerable<SportXml> sportXmlList)
    {
        if (sportXmlList.Any(
            s => s.Sport.Equals("0", StringComparison.OrdinalIgnoreCase)
            && s.League.Equals("0", StringComparison.OrdinalIgnoreCase)))
        {
            return ("0", "0");
        }

        var sportTypes = new HashSet<string>();
        var leagues = new HashSet<string>();

        foreach (var item in sportXmlList)
        {
            foreach (var sport in item.Sport.ToList())
            {
                if (item.League.Equals("0", StringComparison.OrdinalIgnoreCase))
                {
                    sportTypes.Add(sport);
                }
                else
                {
                    foreach (var league in item.League.ToList())
                    {
                        leagues.Add(league);
                    }
                }
            }
        }

        return (string.Join(',', sportTypes), string.Join(',', leagues));
    }

    private static string BuildMembers(byte filterByMembers, string members, string myWatchlistMembers)
    {
        if (filterByMembers == FilterByMembersConstants.AllMembers)
        {
            return members;
        }

        if (filterByMembers == FilterByMembersConstants.AllWatchlistMembers)
        {
            return members;
        }

        return $"{members},{myWatchlistMembers}".Trim(',');
    }

    private static SavedFilter BuildUnsavedFilter(AddFilterParam customFilter)
    {
        var (sportTypes, leagues) = BuildSportTypesAndLeagues(customFilter.Sports);

        return new SavedFilter(
            0,
            Resources.UnsavedFilter,
            false,
            customFilter.BetType,
            customFilter.Stake,
            customFilter.LiveStatus,
            sportTypes,
            leagues,
            BuildMembers(customFilter.FilterByMembers, customFilter.InputList, customFilter.MyWatchlist),
            customFilter.FilterByMembers);
    }
}