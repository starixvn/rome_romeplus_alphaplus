using MediatR;

namespace Alpha.Main.Site.Features.Language
{
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
                message.DisabledVietnamese ? Constants.Language.CodeNamesWithoutVietNamese : Constants.Language.CodeNames,
                Standardize(message.LanguageCode));

            return Task.FromResult(viewModel);
        }

        private static string Standardize(string languageCode)
            => string.IsNullOrWhiteSpace(languageCode) || !Constants.Language.CodeNames.Keys.Contains(languageCode)
            ? Constants.Language.Default
            : languageCode;
    }
}