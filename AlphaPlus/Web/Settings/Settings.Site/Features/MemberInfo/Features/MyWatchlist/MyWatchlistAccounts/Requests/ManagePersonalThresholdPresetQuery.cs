using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class ManagePersonalThresholdPresetQuery : MemberInfoBaseQuery, IRequest<ManagePersonalThresholdPresetViewModel>
    {
    }
}