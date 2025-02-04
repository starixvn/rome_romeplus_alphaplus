using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class SelectDownlineListByStatusCriteria(
        int custId,
        int pageIndex,
        byte roleId,
        byte statusId,
        int totalPages,
        bool isDirectMember) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public int PageIndex { get; } = pageIndex;

        public byte RoleId { get; } = roleId;

        [SpParam(SqlParamName = "Status")]
        public byte StatusId { get; } = statusId;

        public int TotalPages { get; } = totalPages;

        public bool IsDirectMember { get; } = isDirectMember;

        public override string GetSettingKey() => "Age_CustomerSelListByStatus";

        public override bool IsValid() => CustId > 0;
    }
}