using Alpha.Main.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;

namespace Alpha.Main.Site.ViewModels.Advertisement
{
    public class MyWatchlistViewModel
    {
        public MyWatchlistViewModel(string language, string darkModeName, byte roleId)
        {
            var mainLanguage = Main.Constants.Language.MapToMainLanguageCode(language);
            var version = AppSettings.JsVersion;
            DesktopImageName = $"Desktop{darkModeName}_{mainLanguage}.jpg?v={version}";
            MobileImageName = $"Mobile{darkModeName}_{mainLanguage}.jpg?v={version}";
            CustLevel = Enumeration.FromValue<UserLevel>(roleId);
        }

        public string DesktopImageName { get; }

        public string MobileImageName { get; }

        public string UserGuideLink => AppSettings.IsMBCSite
            ? MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=179067")
            : MicroSites.MessageUrl("~/StaticMessage/Guide?id=179067");

        public UserLevel CustLevel { get; }
    }
}