namespace AlphaPlus.Features.Messages.API.Criteria.NewMessage
{
    public class GetUnreadMessagesOnUserCriteria : OnUserCriteriaBase
    {
        public GetUnreadMessagesOnUserCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_MessageNoticeSelByCustID";

        public override bool IsValid() => base.IsValid() && CustId > 0 ;
    }
}
