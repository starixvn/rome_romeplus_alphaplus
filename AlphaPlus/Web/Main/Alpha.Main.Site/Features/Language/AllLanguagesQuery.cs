namespace Alpha.Main.Site.Features.Language
{
    using AlphaPlus.Core.Models;
    using Alpha.Main.Configurations;
    using MediatR;

    public class AllLanguagesQuery : IRequest<LanguageViewModel>
    {
        private readonly string languageCode;

        public AllLanguagesQuery(string languageCode, bool? disabledVietnamese)
        {
            this.languageCode = languageCode ?? new UserSessionExt().Language;
            DisabledVietnamese = disabledVietnamese ?? AppSettings.DisabledVietnamese;
        }

        public string LanguageCode
        {
            get
            {
                return languageCode;
            }
        }

        public bool DisabledVietnamese { get; }
    }
}