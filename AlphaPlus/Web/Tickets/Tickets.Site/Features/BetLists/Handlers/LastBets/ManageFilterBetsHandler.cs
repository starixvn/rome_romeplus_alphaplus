using AlphaPlus.BetLists.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.Models;
using AlphaPlus.Tickets.Site.Features.BetLists.Requests.LastBets;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.LastBets;
using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
using AlphaPlus.Tickets.Site.Features.Shared.Models;
using MediatR;
using Microsoft.Extensions.Options;
using _Result = AlphaPlus.BetLists.Shared.Models.Result;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.LastBets;

public class ManageFilterBetsHandler(
    IOptions<AppSettings> options,
    IUser user,
    IFilterBetsService filterBetsService) :
        IRequestHandler<FilterBySportsQuery, IEnumerable<Sport>>,
        IRequestHandler<GetMyWatchlistMembersQuery, IEnumerable<MyWatchlistMember>>,
        IRequestHandler<ApplyFilterCommand, _Result>,
        IRequestHandler<AddFilterCommand, _Result>,
        IRequestHandler<ManageFiltersQuery, ManageFiltersViewModel>,
        IRequestHandler<AddUnsavedFilterCommand, _Result>,
        IRequestHandler<UpdateFilterCommand, _Result>,
        IRequestHandler<DeleteFiltersCommand, IEnumerable<_Result>>
{
    private readonly AppSettings appSettings = options.Value;
    private readonly IUser user = user;
    private readonly IFilterBetsService filterBetsService = filterBetsService;

    public Task<IEnumerable<Sport>> Handle(FilterBySportsQuery message, CancellationToken cancellationToken)
    {
        var param = new GetSportsParam(message.GroupSport, appSettings.LocalTest, appSettings.IsSiteLabel777, message.Site);

        return filterBetsService.GetSports(param);
    }

    public Task<IEnumerable<MyWatchlistMember>> Handle(GetMyWatchlistMembersQuery message, CancellationToken cancellationToken)
        => filterBetsService.GetMyWatchlistMembers(message.ViewerId);

    public Task<_Result> Handle(ApplyFilterCommand message, CancellationToken cancellationToken)
    {
        user.SetUnsavedFilter(message.ViewerId, message.SubAccId, BuildAddFilterParam(message));
        return Task.FromResult(new _Result());
    }

    public Task<_Result> Handle(AddFilterCommand message, CancellationToken cancellationToken)
        => filterBetsService.AddFilter(BuildAddFilterParam(message));

    public async Task<_Result> Handle(AddUnsavedFilterCommand message, CancellationToken cancellationToken)
    {
        var unsavedFilterParam = user.GetUnsavedFilter(message.ViewerId, message.SubAccId);

        if (unsavedFilterParam == null)
        {
            return new _Result();
        }

        var result = await filterBetsService.AddFilter(new AddFilterParam(
            message.ViewerId,
            message.SubAccId,
            message.FilterName,
            message.IsDefault,
            unsavedFilterParam.BetType,
            unsavedFilterParam.Stake,
            unsavedFilterParam.Sports,
            unsavedFilterParam.LiveStatus,
            unsavedFilterParam.InputList,
            unsavedFilterParam.InputListMembers,
            unsavedFilterParam.MyWatchlist,
            unsavedFilterParam.MyWatchlistMembers,
            unsavedFilterParam.FilterByMembers,
            message.LanguageKey,
            unsavedFilterParam.IP,
            message.Site));

        if (result.ErrCode == 0)
        {
            user.SetUnsavedFilter(message.ViewerId, message.SubAccId, null);
        }

        return result;
    }

    private static AddFilterParam BuildAddFilterParam(AddFilterCommand message)
        => new AddFilterParam(
        message.ViewerId,
        message.SubAccId,
        message.FilterName,
        message.IsDefault,
        CheckIdList(message.BetType),
        message.Stake,
        message.Sports,
        message.LiveStatus,
        CheckIdList(message.InputList),
        message.InputListMembers,
        CheckIdList(message.MyWatchlist),
        message.MyWatchlistMembers,
        message.FilterByMembers,
        message.LanguageKey,
        message.IP,
        message.Site);

    private static string CheckIdList(string idList)
        => string.IsNullOrWhiteSpace(idList) || idList == "0"
        ? idList
        : idList.ToListIntRemoveDuplicate().JoinWithComma();

    public async Task<ManageFiltersViewModel> Handle(ManageFiltersQuery message, CancellationToken cancellationToken)
        => new ManageFiltersViewModel(
            await filterBetsService.GetFilters(
                message.ViewerId,
                message.SubAccId,
                message.Site,
                user.GetUnsavedFilter(message.ViewerId, message.SubAccId)),
            message);

    public Task<_Result> Handle(UpdateFilterCommand message, CancellationToken cancellationToken)
        => filterBetsService.UpdateFilter(new UpdateFilterParam(
        message.ViewerId,
        message.SubAccId,
        message.FilterId,
        message.FilterName,
        message.IsDefault,
        message.LanguageKey,
        message.IP,
        message.Site));

    public async Task<IEnumerable<_Result>> Handle(DeleteFiltersCommand message, CancellationToken cancellationToken)
    {
        var results = new List<_Result>();
        var filterIds = message.Ids.ToListIntRemoveDuplicate();

        foreach (var id in filterIds)
        {
            if (id == 0)
            {
                user.SetUnsavedFilter(message.ViewerId, message.SubAccId, null);
                results.Add(new _Result());
            }
            else
            {
                results.Add(await filterBetsService.DeleteFilter(
                    new DeleteFilterParam(message.ViewerId, message.SubAccId, id, message.LanguageKey, message.IP, message.Site)));
            }
        }

        return results;
    }
}
