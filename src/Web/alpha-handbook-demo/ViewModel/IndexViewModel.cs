using Alpha.Handbook.Demo.Constants;
using Alpha.Handbook.Demo.Helpers;
using Alpha.Handbook.Demo.Models;

namespace Alpha.Handbook.Demo.ViewModels
{
    public class IndexViewModel
    {
        public string LanguageKey { get; set; }
        public int OnUserId { get; set; }

        public bool SupportVietnamese => AppSettingsHelper.SupportVietnamese;
        public string LanguageName => LanguageConsts.LanguageNames[LanguageKey];
        public string Locale => LanguageConsts.Locales[LanguageKey];
        public string LanguageCode => LanguageConsts.Codes[LanguageKey];
        public string MainRootPath => "~/main";
        public string MessageRootPath => "~/message";

        public BriefProfileViewModel BriefProfile { get; set; }

        public IEnumerable<CategoryModel> Categories { get; set; }

        public int CustId { get; set; }
        public int SubAccId { get; set; }
        public int UserId { get; set; }
    }
}