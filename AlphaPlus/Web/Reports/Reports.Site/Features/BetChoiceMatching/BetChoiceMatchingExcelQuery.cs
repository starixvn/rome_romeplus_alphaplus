using AlphaPlus.Reports.Features.BetChoiceMatching;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingExcelQuery : BetChoiceMatchingJsonQuery, IRequest<ExcelFileViewModel>
{
    public string Username { get; set; }

    public decimal? Stake { get; set; }

    public MemberWinLossOperators MemberWinLossOperator { get; set; }

    public decimal? MemberWinLossValue { get; set; }
}