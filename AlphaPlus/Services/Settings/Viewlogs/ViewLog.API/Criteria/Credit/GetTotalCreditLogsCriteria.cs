namespace AlphaPlus.Settings.ViewLogs.API.Criteria.Credit
{
    using System;
    using Fanex.Data.Repository;

    public class GetTotalCreditLogsCriteria : CriteriaBase
    {
        public GetTotalCreditLogsCriteria(
            int userId,
            int userRoleId,
            string custName,
            DateTime fromDate,
            DateTime toDate)
        {
            UserId = userId;
            UserRoleId = userRoleId;
            CustName = custName ?? string.Empty;
            FromDate = fromDate;
            ToDate = toDate;
        }

        public string CustName { get; }

        public int UserId { get; }

        public int UserRoleId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public override string GetSettingKey()
        {
            return "Age_UserLog_CreditSelTotalRecords";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && UserId > 0;
        }
    }
}
