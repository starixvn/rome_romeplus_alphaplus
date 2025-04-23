using Alpha.Announcements.Models;
using Alpha.Announcements.Specs;

namespace AlphaPlus.Features.Messages.API.Models.Announcement
{
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

        public string Content { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public DateTime ReleasedDate { get; set; }

        public bool IsNewAnnouncement { get; set; }

        public IEnumerable<ISpecification<IParam>> Specifications { get; set; } = Enumerable.Empty<ISpecification<IParam>>();
    }
}
