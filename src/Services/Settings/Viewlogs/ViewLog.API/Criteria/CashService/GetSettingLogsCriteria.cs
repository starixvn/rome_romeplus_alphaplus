using Fanex.Data.Repository;

namespace AlphaPlus.Settings.ViewLogs.API.Criteria.CashService
{
    public class GetCashServiceLogsCriteria : CriteriaBase
    {
#pragma warning disable S107 // Methods should not have too many parameters
        public GetCashServiceLogsCriteria(
            int custId,
            string custName,
            int actorId,
            int actionTypeId,
            DateTime fromDate,
            DateTime toDate,
            bool isInternal,
            int pageIndex,
            int pageSize)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            CustId = custId;
            CustName = custName;
            LoginUserId = actorId;
            ActionTypeId = actionTypeId;
            FromDate = fromDate;
            ToDate = toDate;
            IsInternal = isInternal;
            PageSize = pageSize;
            PageIndex = pageIndex;
        }

        public int CustId { get; }

        public string CustName { get; }

        public int LoginUserId { get; }

        public int ActionTypeId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsInternal { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public override string GetSettingKey()
        {
            return "Acc_CashService_UserLog_RunSPSel";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && CustId > 0 && !string.IsNullOrEmpty(CustName);
        }
    }
}
