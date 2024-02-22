using MediatR;

namespace Alpha.Main.Site.Features.Language
{
    using Alpha.Main.Site.Shared.Constants;

    public class LanguageHandler(ILanguageService languageService) :
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>,
        IRequestHandler<ChangeLanguageCommand, string>
    {
        private readonly ILanguageService languageService = languageService;

        public async Task<string> Handle(ChangeLanguageCommand message, CancellationToken cancellationToken)
        {
            var languageCode = Standardize(message.LanguageCode);
            var languageKey = message.ChangeLanguage(languageCode);

            if (message.IsUpdateDisplayLanguage)
            {
                await languageService.UpdateUsageLanguage(message.CustId, message.SubAccId, languageKey, message.IP);
                message.SetDisplayLanguage(languageCode);
            }

            return languageCode;
        }

        public Task<LanguageViewModel> Handle(AllLanguagesQuery message, CancellationToken cancellationToken)
        {
            var viewModel = new LanguageViewModel(
                message.DisabledVietnamese ? Language.CodeNamesWithoutVietNamese : Language.CodeNames,
                Standardize(message.LanguageCode));

            return Task.FromResult(viewModel);
        }

        private static string Standardize(string languageCode)
            => string.IsNullOrWhiteSpace(languageCode) || !Language.CodeNames.Keys.Contains(languageCode)
            ? Language.Default
            : languageCode;
    }
}