using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public class IndexViewModel
    {
        private readonly IUserSession userSession;

        public IndexViewModel(IUserSession userSession, bool mobileView)
        {
            this.userSession = userSession;
            MobileView = mobileView;
        }

        public string ActorLevel => Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName;

        public bool MobileView { get; }
    }
}