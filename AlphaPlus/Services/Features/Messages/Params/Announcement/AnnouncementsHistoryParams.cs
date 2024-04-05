namespace AlphaPlus.Features.Messages.Params.Announcement
{
    using System;

    public class AnnouncementsHistoryParams : BaseAnnouncementsParams
    {        
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }        
    }
}
