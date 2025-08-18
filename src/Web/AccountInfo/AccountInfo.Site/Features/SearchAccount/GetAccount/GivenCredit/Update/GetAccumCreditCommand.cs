using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class GetAccumCreditCommand(int custId) : NonQueryCommand
    {
        public int CustId { get; } = custId;

        public decimal AccumCreditReturn { get; set; }

        public override string GetSettingKey()
            => "Age_CashAccumCredit_AgSel";

        public override bool IsValid() => CustId > 0;
    }
}
