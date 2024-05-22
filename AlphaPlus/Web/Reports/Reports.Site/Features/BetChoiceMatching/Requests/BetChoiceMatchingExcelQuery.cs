using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetChoiceMatching;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingExcelQuery : BetChoiceMatchingJsonQuery, IRequest<ExcelFileViewModel>
{
    public string Username { get; set; }

    public decimal? Stake { get; set; }

    public MemberWinLossOperators MemberWinLossOperator { get; set; }

    public decimal? MemberWinLossValue { get; set; }
}