namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class SelectClosedReasonCriteria(int custId, byte roleId) : AccountInfoCriteriaBase(custId)
    {
        public byte RoleId { get; } = roleId;

        public override string GetSettingKey()
            => "Age_ClosedReasonSelByCust";

        public override bool IsValid()
            => base.IsValid() && RoleId > 0;
    }
}