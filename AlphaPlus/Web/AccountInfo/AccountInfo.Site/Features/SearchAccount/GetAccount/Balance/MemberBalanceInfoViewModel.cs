using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Models;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance
{
    public class MemberBalanceInfoViewModel : BalanceInfoViewModelBase
    {
        private readonly MemberBalanceInfoModel balanceInfoModel;

        public MemberBalanceInfoViewModel(
            CustomerInfoQuery message,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
            : base(message, customerInfo, creditInfo, isAllDayBalanceInfo)
        {
            this.balanceInfoModel = (MemberBalanceInfoModel)balanceInfoModel;
        }

        public override decimal TotalBalance => RoundUpNumber(balanceInfoModel.CustomerCash);

        public override decimal YesterdayBalance => RoundUpNumber(balanceInfoModel.YesterdayCustomerCash);

        public override decimal Outstanding => RoundUpNumber(balanceInfoModel.CustomerOutstanding);

        public decimal NetPosition => RoundDownNumber(balanceInfoModel.NetPosition);

        public override decimal GivenCredit => RoundUpNumber(creditInfo.Credit);

        public decimal BetCredit => RoundDownNumber(creditInfo.Credit + balanceInfoModel.Balance + balanceInfoModel.NetPosition);

        public override decimal GivenCreditMax => creditInfo.MemMaxCreditMax;

        public override decimal PendingTransfer3rd => balanceInfoModel.PendingTransfer3rd;

        public override decimal GivenCreditMin => Math.Max(0, Outstanding - TotalBalance + PendingTransfer3rd);

        internal override bool ShowTransferLink => ActorLevel.IsAgentLevel() && base.ShowTransferLink;
    }
}