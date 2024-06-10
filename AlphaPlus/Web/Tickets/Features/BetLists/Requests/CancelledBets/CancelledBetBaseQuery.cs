// using AlphaPlus.Core.Utils;
// using AlphaPlus.Tickets.Site.Features.BetLists.Models;
// using AlphaPlus.Tickets.Site.Features.Shared.Extensions;
// using MediatR;

// namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CancelledBets;

// public class CancelledBetBaseQuery : BetListBaseQuery, IRequest<CancelledBetViewModel>
// {
//     protected CancelledBetBaseQuery() : base()
//     {
//         FromDate = FromDate == DateTime.MinValue ? SystemTime.Today : FromDate;
//         ToDate = ToDate == DateTime.MinValue ? SystemTime.Today : ToDate;
//         StatusIds = string.IsNullOrEmpty(StatusIds) ? TicketStatusIdConstants.All.Join() : StatusIds;
//     }

//     public string StatusIds { get; set; }

//     public int UserSelectedDateRangeOption { get; set; }

//     public int PageIndex { get; set; }

//     public int PageSize { get; set; }

//     public bool IsFilterVisible { get; set; } = true;

//     public DateTime MinDate { get; set; }
// }