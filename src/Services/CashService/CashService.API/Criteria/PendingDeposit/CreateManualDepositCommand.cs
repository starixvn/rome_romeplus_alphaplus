using AlphaPlus.CashService.API.Models;
using Fanex.Data;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit
{
    public class CreateManualDepositCommand : BaseNonQueryCommand
    {
        public int CustId { get; set; }

        public int UplineId { get; set; }

        public int SubAccId { get; set; }

        public decimal AgentGivenCredit { get; set; }

        public decimal MemberUsedCredit { get; set; }

        public decimal MinLimitAmt { get; set; }

        public decimal MaxLimitAmt { get; set; }

        public string PaymentFrom { get; set; }

        public string PaymentTo { get; set; }

        public decimal Amount { get; set; }

        public byte[] PaySlip { get; set; }

        public string Ip { get; set; }

        public string Remark { get; set; }

        [SpParam(Ignored = true)]
        public int TransId { get; set; }

        [SpParam(Ignored = true)]
        public string CashAccumCreditTbl { get; set; }

        public override string GetSettingKey() => "Age_CashDeposit_ManualAdd";

        public override bool IsValid() => true;
    }
}
