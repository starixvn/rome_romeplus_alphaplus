using AlphaPlus.Reports.Site.Features.WinLossDetail.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail.Requests;

public class CashFeeQuery : WinLossDetailQuery, IRequest<CashFeeViewModel>
{
    private const string CashServiceBillingProductId = "111";

    public string ProductId { get; } = CashServiceBillingProductId;
}
