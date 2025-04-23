namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class SelectMemberListCriteria : SelectCustomerListCriteriaBase
    {
        public SelectMemberListCriteria(
            int userId,
            int uplineId,
            string username,
            bool? enabled,
            bool? closed,
            bool? suspended,
            bool locked,
            byte doubleCommLosing,
            int autoTransfer,
            int pageSize,
            int pageIndex) : base(username,
            enabled,
            closed,
            suspended,
            locked,
            doubleCommLosing,
            autoTransfer,
            pageSize,
            pageIndex)
        {
            UplineId = uplineId;
            UserId = userId;
        }

        public int UserId { get; }

        public int UplineId { get; }

        public override string GetSettingKey() => "Age_CustomerSelByAg";

        public override bool IsValid() => UplineId > 0;
    }
}