namespace AlphaPlus.Settings.ViewLogs.API.Criteria.Credit
{
    using System;
    using Fanex.Data.Repository;

    public class GetCreditLogsCriteria : CriteriaBase
    {
        public GetCreditLogsCriteria(
            int userId,
            int userRoleId,
            string custName,
            DateTime fromDate,
            DateTime toDate,
            int pageIndex,
            int pageSize)
        {
            UserId = userId;
            UserRoleId = userRoleId;
            CustName = custName ?? string.Empty;
            FromDate = fromDate;
            ToDate = toDate;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int UserId { get; }

        public int UserRoleId { get; }

        public string CustName { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public override string GetSettingKey()
        {
            return "Age_UserLog_CreditSel";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && UserId > 0 && UserRoleId > 0;
        }
    }
}
