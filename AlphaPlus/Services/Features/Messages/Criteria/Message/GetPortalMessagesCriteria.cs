namespace AlphaPlus.Features.Messages.API.Criteria.Message
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetPortalMessagesCriteria : CriteriaBase
    {
        public GetPortalMessagesCriteria(
            int custId,
            string parentSites,
            int totalMessage,
            bool isTest)
        {
            CustId = custId;
            ParentSites = parentSites;
            TotalMessage = totalMessage;
            IsTest = isTest;
        }

        [SpParam(SqlParamName = "targetId")]
        public int CustId { get; }

        [SpParam(SqlParamName = "targetId2")]
        public string ParentSites { get; }        

        public int TotalMessage { get; }

        public bool IsTest { get; }

        public override string GetSettingKey() => "Age_CustomerSelMessages";

        public override bool IsValid() => CustId > 0 && !string.IsNullOrEmpty(ParentSites);
    }
}