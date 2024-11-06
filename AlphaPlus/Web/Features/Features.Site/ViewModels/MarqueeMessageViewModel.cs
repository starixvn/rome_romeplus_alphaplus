using AlphaPlus.Features.Messages.API.Models.Message;

namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    public class MarqueeMessageViewModel
    {
        public MarqueeMessageViewModel(
            GeneralMarqueeMessage publicMessage,
            GeneralMarqueeMessage privateMessage)
        {
            PublicMessage = publicMessage;
            PrivateMessage = privateMessage;
        }

        public GeneralMarqueeMessage PublicMessage { get; }

        public GeneralMarqueeMessage PrivateMessage { get; }

    }
}