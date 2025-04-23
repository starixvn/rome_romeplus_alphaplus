using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.ViewLogs.API.Criteria.Login
{
    public class GetTotalLoginLogsCriteria : CriteriaBase
    {
        public GetTotalLoginLogsCriteria(
            int custId,
            string custName,
            DateTime fromDate,
            DateTime toDate,
            byte logType)
        {
            CustId = custId;
            CustName = custName;
            FromDate = fromDate;
            ToDate = toDate;
            LogType = logType;
        }

        [SpParam(SqlParamName = "UserId")]
        public int CustId { get; }

        [SpParam(SqlParamName = "UserName")]
        public string CustName { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte LogType { get; }

        public override string GetSettingKey()
        {
            return "Acc_UserLog_LoginSelTotalRecords";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && CustId > 0;
        }
    }
}
