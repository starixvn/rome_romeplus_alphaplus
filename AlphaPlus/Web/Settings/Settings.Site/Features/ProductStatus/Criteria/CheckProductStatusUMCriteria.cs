using AlphaPlus.Core.Configurations;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.ProductStatus.Criteria
{
    public class CheckProductStatusUMCriteria : CriteriaBase
    {
        public CheckProductStatusUMCriteria(int custId, int? productId = null, int? groupId = null)
            : this(custId.ToString(), productId, groupId)
        {
        }

        protected CheckProductStatusUMCriteria(string custIds, int? productId = null, int? groupId = null)
        {
            ProductId = productId;
            CustIds = custIds;
            GroupId = groupId;
        }

        public int? ProductId { get; }

        public string CustIds { get; }

        public int? GroupId { get; }

        public byte SettingStatus => AppSettings.SettingStatus;

        public override string GetSettingKey()
            => "Acc_CheckUM_MultipleCustProductStatus";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CustIds);
    }
}