using System.Globalization;
using Alpha.Handbook.Demo.Constants;
using Alpha.Handbook.Demo.Helpers;
using Alpha.Handbook.Demo.Requests;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class LanguageHandler :
        IRequestHandler<ChangeLanguageRequest, string>
    {
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture("en-US");

        public Task<string> Handle(ChangeLanguageRequest request, CancellationToken cancellation)
        {
            var languageKey = Standardize(request.Selected);
            var supportVietnamese = AppSettingsHelper.SupportVietnamese;

            if (!supportVietnamese && languageKey == LanguageConsts.VietnameseKey)
            {
                languageKey = LanguageConsts.EnglishKey;
            }

            var code = LanguageConsts.Codes[languageKey];
            AppSettingsHelper.SessionLanguageKey = code;

            return Task.FromResult(languageKey);
        }

        private string Standardize(string languageKey)
            => string.IsNullOrWhiteSpace(languageKey) || !LanguageConsts.LanguageNames.ContainsKey(languageKey)
            ? LanguageConsts.Default
            : languageKey;
    }
}