using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class SelectProductGroupStatusCriteria : CriteriaBase
    {
        private const int defaultGroupTypeId = 160;

        public SelectProductGroupStatusCriteria(int custId, int roleId, int groupId)
        {
            CustId = custId;
            RoleId = roleId;
            GroupId = groupId;
        }

        public int CustId { get; }

        public int RoleId { get; }

        public int GroupId { get; }

        public byte SettingStatus { get; } = AppSettings.SettingStatus;

        public int GroupType { get; } = defaultGroupTypeId;

        public byte Mode { get; } = 1;

        public override string GetSettingKey() => "Acc_CustSetting_StatusSettingSelect";

        public override bool IsValid()
            => GroupId > 0
                && CustId > 0
                && UserLevel.Member.Value <= RoleId
                && RoleId <= UserLevel.Master.Value;
    }
}