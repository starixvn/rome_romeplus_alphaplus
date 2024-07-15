namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers
{
    using System.Threading.Tasks;
    using AlphaPlus.Shared.AppResources.Settings.Transfer;
    using MediatR;
    using Requests.CashBalanceTransfer;
    using ViewModels.CashBalanceTransfer;
    using AlphaPlus.Settings.Transfer.API.Configurations;
    using AlphaPlus.Settings.Transfer.API.Services;
    using AlphaPlus.Core.Models;

    public class CashBalanceTransferHandler :
        IRequestHandler<CashBalanceTransferQuery, CashBalanceTransferViewModel>,
        IRequestHandler<CashBalanceTransferCommand, Result>,
        IRequestHandler<HintTransferQuery, HintTransferViewModel>
    {
        private readonly CashBalanceTransferService cashBalanceTransferService;
        private readonly TransferService transferService;

        public CashBalanceTransferHandler(
            CashBalanceTransferService cashBalanceTransferService,
            TransferService transferService)
        {
            this.cashBalanceTransferService = cashBalanceTransferService;
            this.transferService = transferService;
        }

        public async Task<CashBalanceTransferViewModel> Handle(CashBalanceTransferQuery message, CancellationToken cancellation)
        {
            var model = await cashBalanceTransferService.GetCashBalanceTransferAsync(message.CustId);

            return new CashBalanceTransferViewModel(
                model.UplineCashBalance,
                model.CustId,
                model.UserName,
                Enumeration.FromValue<UserLevel>(model.RoleId),
                model.CashBalance,
                model.Outstanding,
                model.GivenCredit,
                model.PendingTransfer3rd);
        }

        public async Task<Result> Handle(CashBalanceTransferCommand message, CancellationToken cancellation)
        {
            var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.UplineId);
            if (!isAllDayBalTransfer)
            {
                return Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, 6001);
            }

            return await cashBalanceTransferService.AddCashBalanceTransferAsync(
                message.UplineId,
                message.CustId,
                message.Amount,
                message.UplineId,
                message.UplineName,
                message.SubAccId,
                message.IP,
                message.LanguageKey,
                message.AGIP);
        }

        public async Task<HintTransferViewModel> Handle(HintTransferQuery message, CancellationToken cancellation)
        {
            var userRole = Enumeration.FromValue<UserLevel>(message.RoleId);

            var allowTransfer = userRole.IsNotSuperLevel()
                ? await cashBalanceTransferService.GetAllowTransfer(message.CustId)
                : string.Empty;
            var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.CustId);
            var isEnabledAutoTransfer = !isAllDayBalTransfer && AppSettings.CheckOfferAutoTransferCurrencies(message.User.Currency);

            return new HintTransferViewModel(allowTransfer, isAllDayBalTransfer, isEnabledAutoTransfer);
        }
    }
}