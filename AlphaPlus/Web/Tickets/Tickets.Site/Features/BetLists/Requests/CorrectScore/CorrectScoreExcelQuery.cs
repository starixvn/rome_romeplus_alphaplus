using AlphaPlus.Core.Models;
using Fanex.BetList.Core.Entities;
using MediatR;

namespace AlphaPlus.Tickets.Site.Features.BetLists.Requests.CorrectScore;

public class CorrectScoreExcelQuery : CorrectScoreQuery, IRequest<ExcelFileViewModel>
{
    public RTFHelper? RtfHelper { get; set; }
}
