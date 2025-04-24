using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class GlobalThresholdQuery : MemberInfoBaseQuery, IRequest<GlobalThresholdModel>
    {
        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }
    }
}