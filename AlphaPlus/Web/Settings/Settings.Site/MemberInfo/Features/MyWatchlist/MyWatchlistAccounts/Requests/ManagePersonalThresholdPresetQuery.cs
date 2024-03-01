namespace Alpha.MemberInfo.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using Alpha.MemberInfo.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using Alpha.MemberInfo.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ManagePersonalThresholdPresetQuery : MemberInfoBaseQuery, IRequest<ManagePersonalThresholdPresetViewModel>
    {
        public ManagePersonalThresholdPresetQuery(IUser viewer) : base(viewer)
        {
        }
    }
}