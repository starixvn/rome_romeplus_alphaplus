namespace AlphaPlus.Settings.Site.Transfer.Handlers
{
    using AlphaPlus.Settings.Transfer.API.Params.TransferAllAndClearCredit;
    using AlphaPlus.Settings.Transfer.API.Services;
    using MediatR;
    using Requests.TransferAllAndClearCredit;
    using Settings.Transfer.API.Models.TransferAllAndClearCredit;
    using ViewModels.TransferAllAndClearCredit;

    public class TransferAllAndClearCreditHandler :
        IRequestHandler<CalculationQuery, CalculationViewModel>,
        IRequestHandler<TransferResultQuery, TransferResultViewModel>
    {
        private readonly TransferAllAndClearCreditService transferAllAndClearCreditService;
        private readonly TransferService transferService;

        public TransferAllAndClearCreditHandler(
            TransferAllAndClearCreditService transferAllAndClearCreditService,
            TransferService transferService)
        {
            this.transferAllAndClearCreditService = transferAllAndClearCreditService;
            this.transferService = transferService;
        }

        public async Task<CalculationViewModel> Handle(CalculationQuery message, CancellationToken cancellation)
        {
            var isAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);
            var param = new GetCalculationResultParam(
                message.CustId,
                message.Username,
                message.LanguageKey,
                message.AllowedTotalBalanceTransferSuperIds,
                isAllDayBalanceTransfer,
                message.AGIP);

            var calculationResult = string.IsNullOrWhiteSpace(message.Username) ? new TransferResult() : transferAllAndClearCreditService.GetCalculateResult(param);

            return new CalculationViewModel(message, calculationResult);
        }

        public async Task<TransferResultViewModel> Handle(TransferResultQuery message, CancellationToken cancellation)
        {
            var isAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);
            var param = new GetTransferResultParam(
                message.CustId,
                message.UserId,
                message.SubAccId,
                message.IsClearCredit,
                message.Username,
                message.AllowedTotalBalanceTransferSuperIds,
                message.IP,
                message.LanguageKey,
                message.TransferSteps,
                message.ClearCreditSteps,
                isAllDayBalanceTransfer,
                message.AGIP);

            var transferResult = message.IsInternal ? null : transferAllAndClearCreditService.GetTransferResult(param);

            return new TransferResultViewModel(message, param, transferResult);
        }
    }
}