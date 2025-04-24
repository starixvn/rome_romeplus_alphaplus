using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Criteria
{
    public class GetProtectedStatusCriteria : CriteriaBase
    {
        public GetProtectedStatusCriteria(int custId, byte roleId, string custList)
        {
            CustId = custId;
            RoleId = roleId;
            CustList = custList;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public string CustList { get; }

        public override string GetSettingKey()
            => "Age_CustomerSelProtectionModeList";

        public override bool IsValid()
            => CustId > 0
            && (RoleId == UserLevel.Super.Value || RoleId == UserLevel.Master.Value)
            && !string.IsNullOrWhiteSpace(CustList);
    }
}