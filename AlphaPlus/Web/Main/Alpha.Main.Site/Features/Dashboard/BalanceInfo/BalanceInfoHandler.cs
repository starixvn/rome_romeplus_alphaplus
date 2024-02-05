using MediatR;
using Alpha.Main.Services;

namespace Alpha.Main.Site.Features.Dashboard.BalanceInfo
{
    public class BalanceInfoHandler(BalanceInfoService balanceInfoService) : IRequestHandler<BalanceInfoQuery, BalanceInfoViewModel>
    {
        private readonly BalanceInfoService balanceInfoService = balanceInfoService;

        public async Task<BalanceInfoViewModel> Handle(BalanceInfoQuery request, CancellationToken cancellationToken)
        {
            var balanceInfo = await balanceInfoService.GetBalanceInfo(request.CustId, request.UserLevel);

            var viewModel = new BalanceInfoViewModel(
                balanceInfo,
                request.UserLevel,
                request.Currency,
                request.CustId,
                request.CanViewReports,
                request.CanViewProduct);

            return viewModel;
        }
    }
}