using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Models;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance
{
    public class MasterBalanceInfoViewModel : BalanceInfoViewModelBase
    {
        private readonly MasterBalanceInfoModel balanceInfoModel;

        public MasterBalanceInfoViewModel(
            CustomerInfoQuery message,
            CustomerInfoModel account,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
            : base(message, account, creditInfo, isAllDayBalanceInfo)
        {
            this.balanceInfoModel = (MasterBalanceInfoModel)balanceInfoModel;
        }

        public decimal TotalCash => RoundUpNumber(balanceInfoModel.MasterCash);

        public decimal YesterdayCash => RoundUpNumber(balanceInfoModel.YesterdayMasterCash);

        public override decimal TotalBalance
            => RoundUpNumber(
                balanceInfoModel.MasterCash +
                balanceInfoModel.AgentCash +
                balanceInfoModel.CustomerCash);

        public override decimal YesterdayBalance
            => RoundUpNumber(
                balanceInfoModel.YesterdayMasterCash +
                balanceInfoModel.YesterdayAgentCash +
                balanceInfoModel.YesterdayCustomerCash);

        public override decimal Outstanding => RoundUpNumber(balanceInfoModel.MasterPositionTaking);

        public decimal UsedCredit => RoundUpNumber(creditInfo.MasterUsedCredit);

        public override decimal GivenCredit => RoundUpNumber(creditInfo.Credit);

        public override decimal GivenCreditMin => creditInfo.MasterCreditMin;

        public override decimal GivenCreditMax => creditInfo.MasterCreditMax;

        public override decimal PendingTransfer3rd => balanceInfoModel.PendingTransfer3rd;

        public override decimal RewardReceivable => RoundUpNumber(balanceInfoModel.RewardReceivable ?? 0);

        public decimal RewardPayable => RoundUpNumber(balanceInfoModel.RewardPayable ?? 0);

        public override string RewardCurrency => balanceInfoModel.RewardCurrency;
    }
}