using Fanex.Customer.Client.Models;
using MediatR;
using Fanex.Customer.Client.Services;

namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo
{
    using AlphaPlus.Core.Configurations;

    public class BalanceInfoHandler(IBalanceInfoService balanceInfoService, ICustomerService customerService)
        : IRequestHandler<BalanceInfoQuery, BalanceInfoViewModel>
    {
        public async Task<BalanceInfoViewModel> Handle(BalanceInfoQuery request, CancellationToken cancellationToken)
        {
            var balanceInfo = await balanceInfoService.GetBalanceInfo(request.CustId, request.UserLevel);
            var isOfferedCashService = AppSettings.IsOfferedCashService(request.Site) && request.UserLevel.IsAgentLevel();
            var cashServiceInfo = new CashServiceInfo
            {
                IsCashServiceEnabled = false,
                TotalCashMember = new TotalCashMember(),
                IsOfferedCashService = isOfferedCashService
            };
            
            if (isOfferedCashService)
            {
                var isCashServiceEnabled = await customerService.GetCashServiceStatus(request.CustId);
                var totalCashMember = await customerService.GetCashServiceTotalDownline(request.CustId);

                cashServiceInfo = new CashServiceInfo
                {
                    IsCashServiceEnabled = isCashServiceEnabled == 1,
                    TotalCashMember = totalCashMember,
                    IsOfferedCashService = isOfferedCashService
                };
            }


            var viewModel = new BalanceInfoViewModel(
                balanceInfo,
                cashServiceInfo,
                request.UserLevel,
                request.Currency,
                request.CustId,
                request.CanViewReports,
                request.CanViewProduct);

            return viewModel;
        }
    }
}