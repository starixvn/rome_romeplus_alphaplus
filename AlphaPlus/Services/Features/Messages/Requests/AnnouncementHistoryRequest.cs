namespace AlphaPlus.Features.Messages.API.Client.Requests
{
    using System;

    public class AnnouncementHistoryRequest : AnnouncementTodayRequest
    {
        public AnnouncementHistoryRequest(DateTime fromDate, DateTime toDate, string enviroment, string system)
            : base(enviroment, system)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}