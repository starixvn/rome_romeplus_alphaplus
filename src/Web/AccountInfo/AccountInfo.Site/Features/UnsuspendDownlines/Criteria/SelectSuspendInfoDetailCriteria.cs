using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Criteria
{
    public class SelectSuspendInfoDetailCriteria : CriteriaBase
    {
        public SelectSuspendInfoDetailCriteria(
            int custId,
            byte downlineRoleId,
            bool isAffected,
            int pageSize,
            int pageIndex)
        {
            CustId = custId;
            IsAffected = isAffected;
            PageSize = pageSize;
            PageIndex = pageIndex;
            DownlineRoleId = downlineRoleId;
        }

        public int CustId { get; }

        public bool IsAffected { get; }

        public int PageSize { get; }

        public int PageIndex { get; }

        public byte DownlineRoleId { get; }

        public int TotalRows { get; set; }

        public override string GetSettingKey() => "Acc_Customer_SuspendInfoDetailSel";

        public override bool IsValid() => CustId > 0;
    }
}