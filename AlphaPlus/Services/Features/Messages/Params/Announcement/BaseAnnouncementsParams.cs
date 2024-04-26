namespace AlphaPlus.Features.Messages.API.Params.Announcement
{
    using Alpha.Announcements.Models;

    public class BaseAnnouncementsParams
    {
        public int SubAccId { get; set; }

        public byte RoleId { get; set; }

        public int AdminId { private get; set; }

        public bool IsAdminUser => AdminId != 0;

        public int CustId { get; set; }
        
        public string Language { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Site { get; set; } = string.Empty;

        public bool IsMobileDevice { get; set; }

        public string ParentSites { get; set; } = string.Empty;

        public IParam? SpecificationParamter { get; set; }
    }
}
