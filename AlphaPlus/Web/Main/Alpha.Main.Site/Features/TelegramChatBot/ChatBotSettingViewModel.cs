using Alpha.Main.Configurations;
using Alpha.Main.Site.Features.Language;
using AlphaPlus.Core.Helpers;
using TelegramBot.Client.Models;

namespace Alpha.Main.Site.Features.TelegramChatBot
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
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public int UserId { get; }

        public long ChatId { get; }

        public string ChatBotName { get; }

        public IEnumerable<Function> ReportFunctions { get; }

        public IEnumerable<Function> NotificationFunctions { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public LanguageViewModel LanguageViewModel => new(
                AppSettings.DisabledVietnamese ? Constants.Language.TelegramChatBotCodeNamesWithoutVietNamese : Constants.Language.TelegramChatBotCodeNames,
                StandardizeTelegramBotCodeNames(Language));

        private static string StandardizeTelegramBotCodeNames(string languageCode)
            => string.IsNullOrWhiteSpace(languageCode) || !Constants.Language.TelegramChatBotCodeNames.ContainsKey(languageCode)
            ? Constants.Language.Default
            : languageCode;

        public string HowToUseLink
            => AppSettings.IsSiteLabel789y ?
                MicroSites.MessageUrl("~/StaticMessage/Guide?id=167819") :
                MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=167819");
    }
}