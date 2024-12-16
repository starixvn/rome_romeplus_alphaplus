using TelegramBot.Client.Models;

namespace AlphaPlus.Main.Site.Features.TelegramChatBot
{
    public class ChatBotViewModel
    {
        public ChatBotViewModel(CustBotMessenger custBotMessenger, bool isEnableAutoTransfer = false)
        {
            CustBotMessenger = custBotMessenger;
            IsEnableAutoTransfer = isEnableAutoTransfer;
        }

        public CustBotMessenger CustBotMessenger { get; }
 
        public bool IsEnableAutoTransfer { get; }

        public bool IsLinked => CustBotMessenger != null && CustBotMessenger.IsVerifyOTP && CustBotMessenger.Enabled;
    }
}