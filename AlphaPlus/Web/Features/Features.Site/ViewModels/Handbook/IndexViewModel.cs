using AlphaPlus.Core.Helpers;
using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Helpers;
using AlphaPlus.Features.Handbook.API.Models;

namespace AlphaPlus.Features.Site.Handbook.ViewModels
{
    public class IndexViewModel
    {
        public string LanguageKey { get; set; }
        public int OnUserId { get; set; }

        public bool SupportVietnamese => AppSettingsHelper.SupportVietnamese;
        public string LanguageName => LanguageConsts.LanguageNames[LanguageKey];
        public string Locale => LanguageConsts.Locales[LanguageKey];
        public string LanguageCode => LanguageConsts.Codes[LanguageKey];
        public string MainRootPath => MicroSites.GetMicroSiteUrl("Main", "~/");
        public string MessageRootPath => MicroSites.GetMicroSiteUrl("Message", "~/");

        public BriefProfileViewModel BriefProfile { get; set; }

        public IEnumerable<CategoryModel> Categories { get; set; }

        public int CustId { get; set; }
        public int SubAccId { get; set; }
        public int UserId { get; set; }
    }
}