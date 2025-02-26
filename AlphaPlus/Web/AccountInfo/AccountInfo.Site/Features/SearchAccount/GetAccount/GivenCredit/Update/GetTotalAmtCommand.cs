using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update
{
    public class GetTotalAmtCommand(int downlineId) : NonQueryCommand
    {
        public int CustId { get; } = downlineId;

        public decimal TotalAmt { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Acc_CustomerTotalAmountSel";

        public override bool IsValid() => CustId > 0;
    }
}