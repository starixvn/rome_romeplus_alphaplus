namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ManagePersonalThresholdPresetQuery : MemberInfoBaseQuery, IRequest<ManagePersonalThresholdPresetViewModel>
    {
        public ManagePersonalThresholdPresetQuery(IUser viewer) : base(viewer)
        {
        }
    }
}