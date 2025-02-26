using MediatR;
using AlphaPlus.Core.Models;
using AlphaPlus.Features.Messages.API.Models.Message;
using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Params.Announcement;

namespace AlphaPlus.Features.Site.Messages.Requests.NewMessage
{
    public class NumberOfNewMessageQuery : MessageHistoryBaseRequest, IRequest<NumberOfUnreadMessage>
    {
        public NumberOfNewMessageQuery() : this(null) { }

        public NumberOfNewMessageQuery(IUserSession userSession) : base(userSession)
        {
        }

        public NumberOfUnreadMessage UnreadMessage
        {
            get
            {
                return userSession.GetProperty<NumberOfUnreadMessage>(SessionKey.NumberOfUnreadMessage);
            }
            set
            {
                userSession.SetProperty(SessionKey.NumberOfUnreadMessage, value);
            }
        }

        public override BaseAnnouncementsParams ConvertToParameters()
            => new NumberOfNewMessageParams
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
                UnreadMessage = UnreadMessage
            };
    }
}
