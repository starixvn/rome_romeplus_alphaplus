using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class GetSecurityCodeFunctionCriteria(int custId, int subAccId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public int SubAccId { get; } = subAccId;

        public override string GetSettingKey()
            => "Age_CustomerSelSecurityFunctionInfo";

        public override bool IsValid()
            => CustId > 0 && SubAccId >= 0;
    }
}