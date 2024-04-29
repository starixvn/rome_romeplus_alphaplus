using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class SelectDownlineListByStatusCriteria : CriteriaBase
    {
        public SelectDownlineListByStatusCriteria(
            int custId,
            int pageIndex,
            byte roleId,
            byte statusId,
            int totalPages)
        {
            CustId = custId;
            PageIndex = pageIndex;
            RoleId = roleId;
            StatusId = statusId;
            TotalPages = totalPages;
        }

        public int CustId { get; }

        public int PageIndex { get; }

        public byte RoleId { get; }

        [SpParam(SqlParamName = "Status")]
        public byte StatusId { get; }

        public int TotalPages { get; }

        public override string GetSettingKey() => "Age_CustomerSelListByStatus";

        public override bool IsValid() => CustId > 0;
    }
}