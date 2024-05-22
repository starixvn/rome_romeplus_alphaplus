using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetTypeAndMatch;

public class BetListByBetTypeAndMatchExcelQuery : BetListByBetTypeAndMatchQuery, IRequest<ExcelFileViewModel>
{
    public RTFHelper? RtfHelper { get; set; }
}
