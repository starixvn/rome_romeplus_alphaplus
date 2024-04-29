using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.AccountInfo.Requests;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.ViewModels
{
    public class IndexViewModel(IndexQuery message)
    {
        public UserLevel ActorLevel { get; } = message.ActorLevel;

        public bool MobileView { get; } = message.MobileView;
    }
}