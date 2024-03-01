namespace AlphaPlus.Settings.Site.Features.ParlayMaxPayout.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using ViewModels;

    public class ParlayMaxPayoutSingleSelectQuery : MemberInfoBaseQuery, IRequest<ParlayMaxPayoutSinglePageViewModel>
    {
        public ParlayMaxPayoutSingleSelectQuery(IUser viewer) : base(viewer)
        {
        }
    }
}