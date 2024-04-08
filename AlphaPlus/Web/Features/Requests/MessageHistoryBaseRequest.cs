namespace AlphaPlus.Features.Site.Messages.Requests
{
    using Alpha.Announcements.Models;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Messages.API.Constants;
    using AlphaPlus.Features.Messages.API.Params.Announcement;

    public class MessageHistoryBaseRequest
    {
        protected readonly IUserSession userSession;

        public MessageHistoryBaseRequest() : this(null) {}

        public MessageHistoryBaseRequest(IUserSession userSession)
        {
            this.userSession = userSession ?? UserSession.Current;
        }

        public IParam Specification { get; set; }

        public int UserId => userSession.CustId;

        public string ParentSites => MessageConstants.ParentSitesByLevel[userSession.RoleId];

        public int SubAccId => userSession.SubAccId;

        public byte RoleId => userSession.RoleId;

        public int AdminId => userSession.AdminId;

        public bool IsAdminUser => AdminId != 0;

        public int CustId => userSession.CustId;

        public string Language => userSession.Language;

        public string UserName => userSession.UserName;

        public string Site => userSession.Site;

        public bool IsProtectedMode => userSession.IsProtectedMode;

        virtual public BaseAnnouncementsParams ConvertToParameters()
            => new BaseAnnouncementsParams
            {
                AdminId = AdminId,
                CustId = CustId,
                Language = Language,
                RoleId = RoleId,
                Site = Site,
                SubAccId = SubAccId,
                UserName = UserName,
                SpecificationParamter = Specification,
                ParentSites = ParentSites
            };
    }
}