using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Params
{
    public class SelectCloseInfoDetailCriteria : CriteriaBase
    {
        public SelectCloseInfoDetailCriteria(
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

        public override bool IsValid() => CustId > 0;

        public override string GetSettingKey() => "Acc_Customer_CloseInfoDetailSel";
    }
}