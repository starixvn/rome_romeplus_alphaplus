using TelegramBot.Client.Models;

namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class ChatBotViewModel
    {
        public ChatBotViewModel(CustBotMessenger custBotMessenger)
        {
            CustBotMessenger = custBotMessenger;
        }

        public CustBotMessenger CustBotMessenger { get; }

        public bool IsLinked => CustBotMessenger != null && CustBotMessenger.IsVerifyOTP && CustBotMessenger.Enabled;
    }
}