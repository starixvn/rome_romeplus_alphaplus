using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels
{
    public class ReachHistoryViewModel
    {
        public ReachHistoryViewModel(ReachHistoryQuery message, IEnumerable<ReachHistoryModel> reachHistories)
        {
            ReachHistories = reachHistories;
            ReachHistoryQuery = message;
        }

        public IEnumerable<ReachHistoryModel> ReachHistories { get; }

        public ReachHistoryQuery ReachHistoryQuery { get; }
    }
}