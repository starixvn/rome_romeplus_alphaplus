using Alpha.Main.Constants;
using Alpha.Main.Services.Impl;
using MediatR;
using Alpha.Main.Site.Models.Requests;
using Alpha.Main.Site.Requests;
using Alpha.Main.Site.ViewModels;

namespace Alpha.Main.Site.Handlers
{
    public class LanguageHandler :
        IRequestHandler<AllLanguagesQuery, LanguageViewModel>,
        IRequestHandler<TelegramBotLanguageQuery, LanguageViewModel>,
        IRequestHandler<ChangeLanguageCommand, string>
    {
        private readonly UpdateUsageLanguageService updateUsageLanguageService;

        public LanguageHandler(UpdateUsageLanguageService updateUsageLanguageService)
        {
            this.updateUsageLanguageService = updateUsageLanguageService;
        }

        public async Task<string> Handle(ChangeLanguageCommand message, CancellationToken cancellationToken)
        {
            var languageCode = Standardize(message.LanguageCode);
            var languageKey = message.ChangeLanguage(languageCode);

            if (message.IsUpdateDisplayLanguage)
            {
                await updateUsageLanguageService.UpdateUsageLanguage(message.CustId, message.SubAccId, languageKey, message.IP);
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

        public Task<LanguageViewModel> Handle(TelegramBotLanguageQuery message, CancellationToken cancellationToken)
        {
            var viewModel = new LanguageViewModel(
                message.DisabledVietnamese ? Language.TelegramChatBotCodeNamesWithoutVietNamese : Language.TelegramChatBotCodeNames,
                StandardizeTelegramBotCodeNames(message.LanguageCode));

            return Task.FromResult(viewModel);
        }

        private static string Standardize(string languageCode)
            => string.IsNullOrWhiteSpace(languageCode) || !Language.CodeNames.Keys.Contains(languageCode)
            ? Language.Default
            : languageCode;

        private static string StandardizeTelegramBotCodeNames(string languageCode)
            => string.IsNullOrWhiteSpace(languageCode) || !Language.TelegramChatBotCodeNames.Keys.Contains(languageCode)
            ? Language.Default
            : languageCode;
    }
}