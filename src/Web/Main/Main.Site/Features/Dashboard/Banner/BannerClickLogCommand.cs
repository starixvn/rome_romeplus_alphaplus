using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Dashboard.Banner
{
    public class BannerClickLogCommand(string pageUrl, byte roleId) : NonQueryCommand
    {
        public string PageUrl { get; } = pageUrl;

        public byte Role { get; } = roleId;

        public override string GetSettingKey() => "Age_UpdPageVisit";

        public override bool IsValid() => !string.IsNullOrEmpty(PageUrl);
    }
}