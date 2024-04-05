namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    using AlphaPlus.Features.Messages.Models.Message;

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