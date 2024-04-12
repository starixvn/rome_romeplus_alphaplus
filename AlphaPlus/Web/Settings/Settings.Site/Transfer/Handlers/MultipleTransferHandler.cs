namespace AlphaPlus.Settings.Site.Transfer.Handlers
{
    using AlphaPlus.Settings.Transfer.API.Params.MultipleTransfer;
    using AlphaPlus.Settings.Transfer.API.Services;
    using MediatR;
    using Requests.MultipleTransfer;
    using ViewModels.MultipleTransfer;

    public class MultipleTransferHandler :
        IRequestHandler<CustomerListQuery, CustomerListViewModel>
    {
        private readonly MultipleTransferService multipleTransferService;
        private readonly TransferService transferService;

        public MultipleTransferHandler(
            MultipleTransferService multipleTransferService,
            TransferService transferService)
        {
            this.multipleTransferService = multipleTransferService;
            this.transferService = transferService;
        }

        public async Task<CustomerListViewModel> Handle(CustomerListQuery message, CancellationToken cancellation)
        {
            var param = new GetCustomerListParam(
                message.CustLevel,
                message.CustId,
                message.Username,
                message.PageIndex,
                message.PageSize,
                message.AllYesterdayBalance,
                message.AllowTotalBalanceTransfer,
                message.Status);

            var isAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);
            var data = multipleTransferService.GetCustomerList(param);

            return new CustomerListViewModel(message, data, isAllDayBalanceTransfer);
        }
    }
}