using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Accounts.Models;

namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public class MemberBalanceInfoViewModel : BalanceInfoViewModelBase
    {
        private readonly MemberBalanceInfoModel balanceInfoModel;

        public MemberBalanceInfoViewModel(
            IUserSession userSession,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
            : base(userSession, customerInfo, creditInfo, isAllDayBalanceInfo)
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