namespace AlphaPlus.Features.Site.Messages.Requests.MessageHistory
{
    using System;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Features.Messages.Constants;
    using AlphaPlus.Features.Messages.Models.Message;
    using AlphaPlus.Features.Messages.Params;
    using AlphaPlus.Features.Messages.Params.Announcement;
    using MediatR;
    using ViewModels;

    public class MessageHistoryQuery : MessageHistoryBaseRequest, IRequest<MessagesHistoryViewModel>
    {
        public MessageHistoryQuery(
                IUserSession userSession,
                int messageType,
                DateTime startDate,
                DateTime endDate,
                bool isTest, int pageIndex, int pageSize) : base(userSession)
        {
            MessageType = messageType;
            SpecialType = MessageConstants.SpecialTypes[messageType];
            StartDate = MessageType == Features.Messages.Constants.MessageType.Special ? new DateTime(1900, 1, 1) : startDate;
            EndDate = endDate.AddDays(1).AddTicks(-1);
            IsTest = isTest;
            LimitedDateToShowInfo = userSession.GetProperty<DateTime>(SessionKey.CustomerDatelimit);
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int MessageType { get; }

        public int SpecialType { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public bool IsTest { get; }
        
        public int PageIndex { get; }

        public int PageSize { get; }

        public DateTime LimitedDateToShowInfo { get; }

        public NumberOfUnreadMessage NumberOfUnreadMessage
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
            => new MessageHistoryParams(
                     CustId,
                     SubAccId,
                     AdminId,
                     ParentSites,
                     SpecialType,
                     StartDate,
                     EndDate,
                     LimitedDateToShowInfo,
                     IsTest,
                     MessageType,
                     Specification,
                     PageIndex,
                     PageSize)
                    { Site = Site};
    }
}