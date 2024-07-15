using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance.Criteria
{
    public class SelectMemberBalanceCriteria(int custId) : NonQueryCommand
    {
        public int CustId { get; } = custId;

        public decimal Balance { get; set; }

        public decimal NetPosition { get; set; }

        public decimal PendingTransfer3rd { get; set; }

        public override string GetSettingKey() => "SiteDB_Acc_BalanceMemberSel";

        public override bool IsValid() => CustId > 0;
    }
}