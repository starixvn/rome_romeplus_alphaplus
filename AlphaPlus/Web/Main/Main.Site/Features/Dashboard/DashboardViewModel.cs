using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Helpers;

namespace AlphaPlus.Main.Site.Features.Dashboard
{
    public class DashboardViewModel(byte roleId)
    {
        public string UserLevelName { get; } = Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();


        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}
