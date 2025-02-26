using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CasinoDetails;

public class CasinoDetailsExcelQuery : CasinoDetailsQuery, IRequest<ExcelFileViewModel>
{
    public CasinoDetailsExcelQuery() : base()
        => PageSize = int.MaxValue;

    public RTFHelper? RtfHelper { get; set; }
}
