namespace AlphaPlus.Features.Messages.Params
{
    using AlphaPlus.Features.Messages.Constants;
    using Announcement;
    using Alpha.Announcements.Models;
    using System;

    public class MessageHistoryParams : BaseAnnouncementsParams
    {
        public MessageHistoryParams(
            int custId,
            int subAccId,
            int adminId,
            string parentSites,
            int specialType,
            DateTime startDate,
            DateTime endDate,
            DateTime limitedDate,
            bool isTest,
            int messageType,
            IParam specificationParamter,
            int pageIndex = 0,
            int pageSize = 0)
        {
            CustId = custId;
            SubAccId = subAccId;
            AdminId = adminId;
            ParentSites = parentSites;
            SpecialType = specialType;
            StartDate = startDate;
            EndDate = endDate;
            LimitedDate = limitedDate;
            IsTest = isTest;
            MessageType = messageType;
            SpecificationParamter = specificationParamter;
            PageIndex = pageIndex == 0 ? MessageConstants.PageIndex : pageIndex;
            PageSize = GetPageSize(pageSize, messageType);
        }

        private int GetPageSize(int pageSize, int messageType)
        {
            if (messageType == Constants.MessageType.Special)
            {
                return MessageConstants.DefaultTotalSpecialMessageHistory;
            }
            return pageSize != 0 ? pageSize : MessageConstants.PageSize;
        }

        public int SpecialType { get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; }
        public DateTime LimitedDate { get; }
        public bool IsTest { get; }
        public int MessageType { get; }
        public int PageSize { get; }
        public int PageIndex { get; }
    }
}
