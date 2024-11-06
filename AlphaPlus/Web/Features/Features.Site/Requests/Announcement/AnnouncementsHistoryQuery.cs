using MediatR;
using AlphaPlus.Features.Site.Messages.ViewModels;
using AlphaPlus.Core.Models;
using AlphaPlus.Features.Messages.API.Params.Announcement;

namespace AlphaPlus.Features.Site.Messages.Requests.Announcement
{
    public class AnnouncementsHistoryQuery : MessageHistoryBaseRequest, IRequest<AnnouncementHistoryViewModel>
    {
        public AnnouncementsHistoryQuery() : this(null) { }

        public AnnouncementsHistoryQuery(IUserSession userSession) : base(userSession)
        {
            ToDate = DateTime.Now;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        override public BaseAnnouncementsParams ConvertToParameters()
        {
            return new AnnouncementsHistoryParams
            {
                AdminId = AdminId,
                CustId = CustId,
                Language = Language,
                RoleId = RoleId,
                Site = Site,
                ParentSites = ParentSites,
                SubAccId = SubAccId,
                UserName = UserName,
                FromDate = FromDate,
                ToDate = ToDate.AddDays(1).AddSeconds(-1),
                SpecificationParamter = Specification
            };
        }
    }
}
