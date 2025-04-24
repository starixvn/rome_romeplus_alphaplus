using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.DailyStatement;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.DailyStatement;

public class DailyStatementQuery : BetListBaseQuery, IRequest<DailyStatementViewModel>
{
    public DateTime WinLossDate { get; set; }

    public int TransType { get; set; }

    public bool SupportRiskAmount => TransType == 3;
}