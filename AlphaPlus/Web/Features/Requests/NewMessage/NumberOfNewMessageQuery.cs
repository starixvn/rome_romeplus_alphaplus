namespace AlphaPlus.Features.Site.Messages.Requests.NewMessage
{
    using MediatR;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Messages.Models.Message;
    using AlphaPlus.Features.Messages.Constants;
    using AlphaPlus.Features.Messages.Params.Announcement;

    public class NumberOfNewMessageQuery : MessageHistoryBaseRequest, IRequest<NumberOfUnreadMessage>
    {
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
