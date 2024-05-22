namespace AlphaPlus.Settings.Site.Features.CustomerList.Criteria
{
    using Fanex.Data.Repository;

    public class SelectClosedReasonCriteria : CriteriaBase
    {
        public SelectClosedReasonCriteria(int custId, byte roleId)
        {
            CustId = custId;
            RoleId = roleId;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public override string GetSettingKey()
            => "Age_ClosedReasonSelByCust";

        public override bool IsValid()
            => CustId > 0 && RoleId > 0;
    }
}
