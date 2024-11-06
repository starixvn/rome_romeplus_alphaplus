using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Features.QRCodeMemberSignIn.Criteria
{
    public class SelectCreditAndStatusCriteria : CriteriaBase
    {
        public SelectCreditAndStatusCriteria(string searchAccountName)
        {
            SearchAccountName = searchAccountName;
        }

        [SpParam(SqlParamName = "RefCust")]
        public string SearchAccountName { get; }

        public override string GetSettingKey() => "Age_CustomerInsMember_Ag_CreditStatusSel";

        public override bool IsValid() => !string.IsNullOrEmpty(SearchAccountName);
    }
}
