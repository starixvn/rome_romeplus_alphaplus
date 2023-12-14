using Alpha.Main.Site.Requests.TelegramChatBot;
using TelegramBot.Client.Models;

namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class RegisterAccountViewModel
    {
        public RegisterAccountViewModel(ChatBotViewModel chatBotViewModel, SetUpChatBotQuery query)
        {
            ChatBotViewModel = chatBotViewModel;
            Language = query.Language;
            CustLevel = query.CustLevel.DisplayName;
        }

        public ChatBotViewModel ChatBotViewModel { get; }

        public string Language { get; }

        public string CustLevel { get; }

        public Function OtpFunction => ChatBotViewModel.CustBotMessenger.Functions.FirstOrDefault(func => func.FunctionId == 15);
    }
}
