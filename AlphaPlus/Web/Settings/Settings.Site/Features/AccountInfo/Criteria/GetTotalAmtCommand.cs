using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Criteria
{
    public class GetTotalAmtCommand : NonQueryCommand
    {
        public GetTotalAmtCommand(int downlineId)
        {
            CustId = downlineId;
        }

        public int CustId { get; }

        public decimal TotalAmt { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Acc_CustomerTotalAmountSel";

        public override bool IsValid() => CustId > 0;
    }
}