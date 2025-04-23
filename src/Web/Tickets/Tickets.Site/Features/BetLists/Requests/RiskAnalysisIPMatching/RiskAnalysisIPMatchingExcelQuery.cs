using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.RiskAnalysisIPMatching;

public class RiskAnalysisIPMatchingExcelQuery : RiskAnalysisIPMatchingQuery, IRequest<ExcelFileViewModel>
{
    /// <summary>
    /// Use for unit testing
    /// </summary>
    public RTFHelper? RtfHelper { get; set; }
}
