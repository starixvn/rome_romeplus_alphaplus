using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Accounts.Models;

namespace AlphaPlus.Settings.Site.Features.Accounts.ViewModels
{
    public class MasterBalanceInfoViewModel : BalanceInfoViewModelBase
    {
        private readonly MasterBalanceInfoModel balanceInfoModel;

        public MasterBalanceInfoViewModel(
            IUserSession userSession,
            CustomerInfoModel customerInfo,
            CreditInfo creditInfo,
            IBalanceInfoModel balanceInfoModel,
            bool isAllDayBalanceInfo)
            : base(userSession, customerInfo, creditInfo, isAllDayBalanceInfo)
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
    }
}