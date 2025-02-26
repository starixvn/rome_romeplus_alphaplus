namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class SelectMemberListCriteria : SelectCustomerListCriteriaBase
    {
        public SelectMemberListCriteria(
            int userId,
            int recommend,
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
            Recommend = recommend;
            UserId = userId;
        }

        public int UserId { get; }

        public int Recommend { get; }

        public override string GetSettingKey() => "Age_CustomerSelByAg";

        public override bool IsValid() => Recommend > 0;
    }
}