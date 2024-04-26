using AlphaPlus.Features.Messages.API.Models.Message;

namespace AlphaPlus.Features.Messages.API.Params.Announcement
{
    public class NumberOfNewMessageParams : BaseAnnouncementsParams
    {
        public NumberOfUnreadMessage UnreadMessage { get; set; } = new NumberOfUnreadMessage();
    }
}
