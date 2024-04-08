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
        
        public string Language { get; set; }

        public string UserName { get; set; }

        public string Site { get; set; }        

        public bool IsMobileDevice { get; set; }

        public string ParentSites { get; set; }

        public IParam SpecificationParamter { get; set; }
    }
}
