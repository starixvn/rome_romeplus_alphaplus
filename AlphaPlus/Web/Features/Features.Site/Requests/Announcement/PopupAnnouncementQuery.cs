namespace AlphaPlus.Features.Site.Messages.Requests.Announcement
{
    using MediatR;
    using AlphaPlus.Features.Messages.API.Models.Announcement;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Messages.API.Constants;
    using AlphaPlus.Features.Messages.API.Params.Announcement;

    public class PopupAnnouncementQuery : MessageHistoryBaseRequest, IRequest<AlphaAnnouncement>
    {
        public PopupAnnouncementQuery() : this(null) {}
        
        public PopupAnnouncementQuery(IUserSession userSession) : base(userSession)
        {            
        }

        
        public bool IsShowedPopup
        {
            get
            {
                return userSession.GetProperty<bool>(SessionKey.ShowedPopupAnnouncement);
            }
            set
            {        
                userSession.SetProperty(SessionKey.ShowedPopupAnnouncement, value);
            }
        }

        public override BaseAnnouncementsParams ConvertToParameters()
            => new PopupAnnouncementParams
            {
                AdminId = AdminId,
                CustId = CustId,
                Language = Language,
                RoleId = RoleId,
                Site = Site,
                SubAccId = SubAccId,
                UserName = UserName,
                SpecificationParamter = Specification,
                ParentSites = ParentSites,
                IsShowedPopup = IsShowedPopup
            };        
    }
}
