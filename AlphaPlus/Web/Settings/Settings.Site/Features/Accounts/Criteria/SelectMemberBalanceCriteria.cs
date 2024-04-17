using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.Accounts.Criteria
{
    public class SelectMemberBalanceCriteria : NonQueryCommand
    {
        public SelectMemberBalanceCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public decimal Balance { get; set; }

        public decimal NetPosition { get; set; }

        public decimal PendingTransfer3rd { get; set; }

        public override string GetSettingKey() => "SiteDB_Acc_BalanceMemberSel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}