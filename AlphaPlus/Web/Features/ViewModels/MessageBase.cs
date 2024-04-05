namespace AlphaPlus.Features.Site.Messages.ViewModels
{
    using System;
    using AlphaPlus.Features.Messages.Models.Constants;
    using AlphaPlus.Features.Messages.Models.Message;

    public class MessageBase
    {
        public MessageBase(
            int messageType,
            byte roleId,
            DateTime fromDate,
            DateTime toDate,
            NumberOfUnreadMessage unreadMessage)
        {
            RoleId = roleId;
            MessageType = messageType;
            UnreadMessage = unreadMessage ?? new NumberOfUnreadMessage();
            FromDate = fromDate;
            ToDate = toDate;
        }

        public NumberOfUnreadMessage UnreadMessage { get; }

        public int MessageType { get; protected set; }

        public byte RoleId { get; }

        public string RoleName => UserLevel.GetDisplayName(RoleId);

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
        
    }
}