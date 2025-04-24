using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CancelledBets;

public class CancelledBetExcelQuery : CancelledBetQuery, IRequest<ExcelFileViewModel>
{
    public CancelledBetExcelQuery() : base()
    {
        PageIndex = 1;
        PageSize = ushort.MaxValue;
    }

    public RTFHelper? RtfHelper { get; set; }
}
