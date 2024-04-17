using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Accounts.Models;

namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public class AgentBalanceInfoViewModel : BalanceInfoViewModelBase
    {
        private readonly AgentBalanceInfoModel balanceInfoModel;

        public AgentBalanceInfoViewModel(
            IUserSession userSession,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
            : base(userSession, customerInfo, creditInfo, isAllDayBalanceInfo)
        {
            this.balanceInfoModel = (AgentBalanceInfoModel)balanceInfoModel;
        }

        public decimal TotalCash => RoundUpNumber(balanceInfoModel.AgentCash);

        public decimal YesterdayCash => RoundUpNumber(balanceInfoModel.YesterdayAgentCash);

        public override decimal TotalBalance => RoundUpNumber(balanceInfoModel.AgentCash + balanceInfoModel.CustomerCash);

        public override decimal YesterdayBalance => RoundUpNumber(balanceInfoModel.YesterdayAgentCash + balanceInfoModel.YesterdayCustomerCash);

        public override decimal Outstanding => RoundUpNumber(balanceInfoModel.AgentPositionTaking);

        public decimal MemberMaxCredit => RoundUpNumber(creditInfo.MemberMaxCredit);

        public decimal UsedCredit => RoundUpNumber(creditInfo.AgentUsedCredit);

        public override decimal GivenCredit => RoundUpNumber(creditInfo.Credit);

        public bool ShowMemberMaxCreditLimit => ActorLevel.IsMasterLevel();

        // TODO:
        // public string MemberMaxCreditLimit => $">={MemMaxCreditMin} {Resources.And} <={GivenCredit}";

        public override decimal GivenCreditMin => creditInfo.AgentCreditMin;

        public override decimal GivenCreditMax => creditInfo.AgentCreditMax;

        public override decimal PendingTransfer3rd => balanceInfoModel.PendingTransfer3rd;

        internal override bool ShowTransferLink => ActorLevel.IsMasterLevel() && base.ShowTransferLink;

        private decimal MemMaxCreditMin => creditInfo.MemMaxCreditMin;
    }
}