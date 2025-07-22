using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class CashFeeGetDataQuery : WinLossQuery, IRequest<CashFeeViewModel>
{
    public string Username { get; set; }    
}
