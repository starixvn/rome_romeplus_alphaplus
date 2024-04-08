namespace AlphaPlus.Features.Messages.API.Criteria.Message
{
    using AlphaPlus.Features.Messages.API.Configurations;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetNumberOfUnreadMessageCriteria : CriteriaBase
    {
        public GetNumberOfUnreadMessageCriteria(int custId, int subAccId, string parentSites, string site)
        {
            CustId = custId;
            SubAccId = subAccId;
            ParentSites = parentSites;
            Site = site;
            IsTest = AppSettings.LocalTest;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        [SpParam(SqlParamName = "targetId2")]
        public string ParentSites { get; }

        public string Site { get; }

        public bool IsTest { get; }
        
        public override string GetSettingKey() => "Age_MsgCustReadStatus_SelUnread";

        public override bool IsValid() => CustId >= 0;
    }
}
