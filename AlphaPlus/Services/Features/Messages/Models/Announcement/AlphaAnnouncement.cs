namespace AlphaPlus.Features.Messages.Models.Announcement
{
    using Alpha.Announcements.Models;
    using Alpha.Announcements.Specs;
    using System;
    using System.Collections.Generic;

    public class AlphaAnnouncement
    {
        public AlphaAnnouncement()
        {
            Content = string.Empty;
            Code = string.Empty;
        }

        public AlphaAnnouncement(
            int id,
            string content, 
            string code, 
            DateTime releasedDate,
            DateTime lastSeenDate,
            IEnumerable<ISpecification<IParam>> specifications)
        {
            Id = id;
            ReleasedDate = releasedDate;
            Content = content;
            Code = code;
            Specifications = specifications;
            IsNewAnnouncement = releasedDate >= lastSeenDate;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public string Code { get; set; }

        public DateTime ReleasedDate { get; set; }

        public bool IsNewAnnouncement { get; set; }

        public IEnumerable<ISpecification<IParam>> Specifications { get; set; }
    }
}
