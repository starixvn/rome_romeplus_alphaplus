namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ManagePersonalThresholdPresetQuery : MemberInfoBaseQuery, IRequest<ManagePersonalThresholdPresetViewModel>
    {
        public ManagePersonalThresholdPresetQuery() : this(null){}
        
        public ManagePersonalThresholdPresetQuery(IUser viewer) : base(viewer)
        {
        }
    }
}