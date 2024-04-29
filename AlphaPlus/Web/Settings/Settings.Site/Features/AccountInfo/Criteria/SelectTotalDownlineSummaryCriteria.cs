using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class SelectTotalDownlineSummaryCriteria(int custId, byte roleId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public byte RoleId { get; } = roleId;

        public override string GetSettingKey() => "Acc_CustomerSelTotalDownline";

        public override bool IsValid() => CustId > 0;
    }
}