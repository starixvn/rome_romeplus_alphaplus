namespace AlphaPlus.Features.Messages.API.Params.Announcement
{
    public class AnnouncementsHistoryParams : BaseAnnouncementsParams
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
