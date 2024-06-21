using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetListByBetType;

public class BetListByBetTypeExcelQuery : BetListByBetTypeQuery, IRequest<ExcelFileViewModel>
{
    public RTFHelper? RtfHelper { get; set; }
}