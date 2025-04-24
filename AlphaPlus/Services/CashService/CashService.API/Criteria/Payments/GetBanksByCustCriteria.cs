using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetBanksByCustCriteria : CriteriaBase
    {
        public GetBanksByCustCriteria(int custId)
        {
            CustId = custId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; }

        public override string GetSettingKey() => "Age_CashService_UserBankAccount";

        public override bool IsValid() => CustId > 0;
    }
} 