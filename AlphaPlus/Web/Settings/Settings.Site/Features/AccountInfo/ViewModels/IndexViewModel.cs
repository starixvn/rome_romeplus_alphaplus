using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Requests;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.ViewModels
{
    public class IndexViewModel(IndexQuery message, string assetsVersion)
    {
        public UserLevel ActorLevel { get; } = message.ViewerLevel;

        public bool MobileView { get; } = message.MobileView;

        public string ActorName { get; } = message.ViewerUsername;

        public string AssetsVersion { get; } = assetsVersion;

        public bool ShowLoginInfo => !message.IsEnabledCustomerDateLimit;
    }
}