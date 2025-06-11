using Alpha.Handbook.Demo.Constants;
using Alpha.Handbook.Demo.Helpers;

namespace Alpha.Handbook.Demo.Requests
{
    public class BaseHandbookRequest
    {
        public int CustId { get; set; }
        public string LanguageKey => AppSettingsHelper.SessionLanguageKey;

        public string SiteLabel = AppSettingsHelper.SiteLabel;
        public string RoleId = AppSettingsHelper.RoleId;
        public string Environment = AppSettingsHelper.ContentAPIEnvironment;

        public string Language => LanguageConsts.Locales[LanguageKey];
    }
}