using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.ViewLogs.API.Criteria.Login
{
    public class GetLoginLogsCriteria : CriteriaBase
    {
        public GetLoginLogsCriteria(
            int custId,
            string custName,
            DateTime fromDate,
            DateTime toDate,
            byte logType,
            int pageIndex,
            int pageSize)
        {
            CustId = custId;
            CustName = custName;
            FromDate = fromDate;
            ToDate = toDate;
            LogType = logType;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        [SpParam(SqlParamName = "UserId")]
        public int CustId { get; }

        [SpParam(SqlParamName = "UserName")]
        public string CustName { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte LogType { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public override string GetSettingKey()
        {
            return "Acc_UserLog_LoginSel";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && CustId > 0;
        }
    }
}
