using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Criteria
{
    public class SelectClosedReasonCriteria(int custId, byte roleId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public byte RoleId { get; } = roleId;

        public override string GetSettingKey()
            => "Age_ClosedReasonSelByCust";

        public override bool IsValid()
            => CustId > 0 && RoleId > 0;
    }
}