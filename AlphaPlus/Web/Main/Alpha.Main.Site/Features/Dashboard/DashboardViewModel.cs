using AlphaPlus.Core.Models;
using Alpha.Main.Site.Helpers;

namespace Alpha.Main.Site.Features.Dashboard
{
    public class DashboardViewModel(byte roleId)
    {
        public string UserLevelName { get; } = Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();


        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}
