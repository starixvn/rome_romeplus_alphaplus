using AlphaPlus.Main.Site.Features.Language;
using AlphaPlus.Core.Helpers;
using TelegramBot.Client.Models;

namespace AlphaPlus.Main.Site.Features.TelegramChatBot
{
    public class ChatBotSettingViewModel
    {
        public ChatBotSettingViewModel(ChatBotViewModel chatBotViewModel, SetUpChatBotQuery query)
        {
            ChatBotViewModel = chatBotViewModel;

            UserId = chatBotViewModel.CustBotMessenger.UserId;
            ChatId = chatBotViewModel.CustBotMessenger.ChatId;
            ChatBotName = chatBotViewModel.CustBotMessenger.BotName;
            ReportFunctions = chatBotViewModel.CustBotMessenger.Functions.Where(function => function.Type == 1);
            NotificationFunctions = chatBotViewModel.CustBotMessenger.Functions.Where(function => function.Type == 2);
            Language = chatBotViewModel.CustBotMessenger.Language;
            CustLevel = query.CustLevel.DisplayName;
            IsEnableAutoTransfer = chatBotViewModel.IsEnableAutoTransfer;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public int UserId { get; }

        public long ChatId { get; }

        public string ChatBotName { get; }

        public IEnumerable<Function> ReportFunctions { get; }

        public IEnumerable<Function> NotificationFunctions { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public bool IsEnableAutoTransfer { get; }

        public LanguageViewModel LanguageViewModel => new(
                AppSettings.DisabledVietnamese ? Shared.Constants.Language.TelegramChatBotCodeNamesWithoutVietNamese : Shared.Constants.Language.TelegramChatBotCodeNames,
                StandardizeTelegramBotCodeNames(Language));

        private static string StandardizeTelegramBotCodeNames(string languageCode)
            => string.IsNullOrWhiteSpace(languageCode) || !Shared.Constants.Language.TelegramChatBotCodeNames.ContainsKey(languageCode)
            ? Shared.Constants.Language.Default
            : languageCode;

        public string HowToUseLink {
            get
            {
                if (AlphaPlus.Core.Configurations.AppSettings.SupportedNewHandbook)
                {
                    return MicroSites.FeatureUrl($"~/HandbookDefault/GetBySlug?slug=telegram-chatbot");
                }

                return AppSettings.IsSiteLabel789y ?
                    MicroSites.FeatureUrl("~/StaticMessage/Guide?id=167819") :
                    MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=167819");
            }
        }
    }
}