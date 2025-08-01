using AlphaPlus.Features.Messages.API.Constants;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria.Message
{
    public class GetMessagesHistoryCriteria : CriteriaBase
    {
        private static readonly Dictionary<int, string> SettingKeys = new()
        {
            [MessageType.Personal] = "Age_Msg_GetPrivateMessage",
            [MessageType.Normal] = "Age_Msg_GetPublicMessage",
            [MessageType.Special] = "Age_Msg_GetSpecialMessage"
        };

        private readonly string settingKey;

        public GetMessagesHistoryCriteria(
            int custId,
            string parentSites,
            int specialType,
            DateTime startDate,
            DateTime endDate,
            bool isTest,
            int messageType,
            string site,
            int pageIndex,
            int pageSize)
        {
            settingKey = SettingKeys[messageType];
            CustId = custId;
            ParentSites = parentSites;
            Special = specialType;
            PageIndex = pageIndex;
            PageSize = pageSize;
            StartDate = startDate;
            EndDate = endDate;
            IsTest = isTest;
            Site = site;
        }

        [SpParam(SqlParamName = "targetId")]
        public int CustId { get; }

        [SpParam(SqlParamName = "targetId2")]
        public string ParentSites { get; }

        public int Special { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public int TotalRows { get; set; }

        [SpParam(SqlParamName = "sdate")]
        public DateTime StartDate { get; }

        [SpParam(SqlParamName = "edate")]
        public DateTime EndDate { get; }

        public bool IsTest { get; }

        public string Site { get; }

        public override string GetSettingKey() => settingKey;

        public override bool IsValid() => CustId > 0 && !string.IsNullOrEmpty(ParentSites);
    }
}