using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Criteria
{
    public class SelectSuspendInfoMultipleCriteria : CriteriaBase
    {
        public SelectSuspendInfoMultipleCriteria(string custIds, byte roleId)
        {
            CustIds = custIds;
            RoleId = roleId;
        }

        [SpParam(SqlParamName = "CustIdList")]
        public string CustIds { get; }

        public byte RoleId { get; }

        public override string GetSettingKey() => "Acc_CustomerList_SuspendInfoSel";

        public override bool IsValid() => !string.IsNullOrEmpty(CustIds);
    }
}