namespace AlphaPlus.Features.Messages.API.Criteria.Announcement
{
    using Fanex.Data.Repository;

    public class GetLastSeenAnnouncementCriteria : CriteriaBase
    {
        public GetLastSeenAnnouncementCriteria(int custId, int subAccId)
        {
            CustId = custId;
            SubAccId = subAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public override string GetSettingKey() => "Age_MsgCustReadStatus_Sel";

        public override bool IsValid() => CustId > 0;
    }
}
