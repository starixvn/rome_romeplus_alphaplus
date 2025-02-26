using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount
{
    public class IndexViewModel(IndexQuery message)
    {
        public UserLevel ActorLevel { get; } = message.ViewerLevel;

        public bool MobileView { get; } = message.MobileView;

        public string ActorName { get; } = message.ViewerName;

        public bool ShowLoginInfo => !message.IsEnabledCustomerDateLimit;
    }
}