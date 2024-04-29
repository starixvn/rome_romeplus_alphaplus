using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class SelectMemberOutstandingCriteria : NonQueryCommand
    {
        public SelectMemberOutstandingCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public decimal Outstanding { get; set; }

        public override string GetSettingKey() => "Acc_OutstandingMemberSel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}