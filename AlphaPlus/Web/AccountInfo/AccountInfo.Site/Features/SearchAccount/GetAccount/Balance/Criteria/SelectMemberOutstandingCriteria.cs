using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance.Criteria
{
    public class SelectMemberOutstandingCriteria(int custId) : NonQueryCommand
    {
        public int CustId { get; } = custId;

        public decimal Outstanding { get; set; }

        public override string GetSettingKey() => "Acc_OutstandingMemberSel";

        public override bool IsValid() => CustId > 0;
    }
}