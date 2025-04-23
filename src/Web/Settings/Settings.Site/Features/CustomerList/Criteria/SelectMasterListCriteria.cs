namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    public class SelectMasterListCriteria : SelectCustomerListCriteriaBase
    {
        public SelectMasterListCriteria(
            int srecommend,
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
            SRecommend = srecommend;
        }


        public int SRecommend { get; }

        public override string GetSettingKey() => "Age_CustomerSelBySAg";

        public override bool IsValid() => SRecommend > 0;
    }
}