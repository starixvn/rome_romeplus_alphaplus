using TelegramBot.Client.Models;

namespace AlphaPlus.Main.Site.Features.TelegramChatBot
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