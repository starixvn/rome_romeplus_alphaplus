using AlphaPlus.Core.Models;
using Alpha.Main.Site.Helpers;

namespace Alpha.Main.Site.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public DashboardViewModel(byte roleId)
        {
            UserLevelName = Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();
        }

        public string UserLevelName { get; }


        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}
