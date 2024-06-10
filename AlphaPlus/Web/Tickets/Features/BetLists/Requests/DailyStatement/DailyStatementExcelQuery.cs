using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.DailyStatement;

public class DailyStatementExcelQuery : DailyStatementQuery, IRequest<ExcelFileViewModel>
{
    /// <summary>
    /// Use for unit testing
    /// </summary>
    public RTFHelper? RtfHelper { get; set; }
}