using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class ManageGlobalThresholdQuery : MemberInfoBaseQuery, IRequest<ManageGlobalThresholdViewModel>
    {
        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }
    }
}