namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class MyThresholdCommand : UpdateGlobalThresholdCommand, IRequest<Result>
    {
        public MyThresholdCommand(IUser viewer) : base(viewer)
        {
        }
    }
}