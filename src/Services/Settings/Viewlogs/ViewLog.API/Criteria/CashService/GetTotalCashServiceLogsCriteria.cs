using Fanex.Data.Repository;

namespace AlphaPlus.Settings.ViewLogs.API.Criteria.CashService
{
    public class GetTotalCashServiceLogsCriteria : CriteriaBase
    {
        public GetTotalCashServiceLogsCriteria(
            int custId,
            int loginUserId,
            int actionTypeId,
            DateTime fromDate,
            DateTime toDate,
            bool isInternal)
        {
            CustId = custId;
            LoginUserId = loginUserId;
            ActionTypeId = actionTypeId;
            FromDate = fromDate;
            ToDate = toDate;
            IsInternal = isInternal;
        }

        public int CustId { get; set; }

        public int LoginUserId { get; }

        public int ActionTypeId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsInternal { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CashService_UserLog_RunSPSelTotalRecords";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && CustId > 0;
        }
    }
}
