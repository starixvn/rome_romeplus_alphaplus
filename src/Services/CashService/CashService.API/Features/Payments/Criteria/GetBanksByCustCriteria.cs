using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetBanksByCustCriteria(int custId) : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int CustId { get; } = custId;

        public override string GetSettingKey() => "Age_CashService_UserBankAccount";

        public override bool IsValid() => CustId > 0;
    }
} 