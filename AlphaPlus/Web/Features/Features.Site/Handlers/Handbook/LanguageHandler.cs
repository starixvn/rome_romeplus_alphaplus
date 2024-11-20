using System.Globalization;
using AlphaPlus.Core.Models;
using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Helpers;
using AlphaPlus.Features.Site.Handbook.Requests;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class LanguageHandler :
        IRequestHandler<ChangeLanguageRequest, string>
    {
        private readonly CultureInfo defaultCulture = CultureInfo.CreateSpecificCulture("en-US");
        private readonly IUserSession userSession;

        public LanguageHandler(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public async Task<string> Handle(ChangeLanguageRequest request, CancellationToken cancellation)
        {
            var languageKey = Standardize(request.Selected);
            var supportVietnamese = AppSettingsHelper.SupportVietnamese;

            if (!supportVietnamese && languageKey == LanguageConsts.VietnameseKey)
            {
                languageKey = LanguageConsts.EnglishKey;
            }

            var code = LanguageConsts.Codes[languageKey];
            userSession.Language = code;
            userSession.LangKey = languageKey;

            return languageKey;
        }

        private string Standardize(string languageKey)
            => string.IsNullOrWhiteSpace(languageKey) || !LanguageConsts.LanguageNames.ContainsKey(languageKey)
            ? LanguageConsts.Default
            : languageKey;
    }
}