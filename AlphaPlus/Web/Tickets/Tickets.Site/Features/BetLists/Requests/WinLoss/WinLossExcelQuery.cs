using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.WinLoss;

public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
{
    public WinLossExcelQuery() => IsExcel = true;

    /// <summary>
    /// Use for unit testing
    /// </summary>
    public RTFHelper? RtfHelper { get; set; }
}
