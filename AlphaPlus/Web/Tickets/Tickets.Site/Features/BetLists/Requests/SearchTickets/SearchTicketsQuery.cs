using AlphaPlus.BetLists.Shared.Services;
using AlphaPlus.Tickets.Site.Features.BetLists.ViewModels.SearchTickets;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.SearchTickets;

public class SearchTicketsQuery : BetListBaseQuery, IRequest<SearchTicketsViewModel>
{
    public SearchTicketsQuery() : base()
        => MinDate = ReportDateService.Instance.GetBalanceUpDay().GetAwaiter().GetResult();

    public long RefNo { get; set; }

    public DateTime MinDate { get; }
}