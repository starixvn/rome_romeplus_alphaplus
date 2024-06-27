namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.ParlayMaxPayout.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using ViewModels;

    public class ParlayMaxPayoutSingleSelectQuery : MemberInfoBaseQuery, IRequest<ParlayMaxPayoutSinglePageViewModel>
    {
        public ParlayMaxPayoutSingleSelectQuery() : this(null){}
        
        public ParlayMaxPayoutSingleSelectQuery(IUser viewer) : base(viewer)
        {
        }
    }
}