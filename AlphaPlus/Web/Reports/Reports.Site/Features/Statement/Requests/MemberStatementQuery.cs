using AlphaPlus.Reports.Features.Shared.Extensions;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class MemberStatementQuery : SMAStatementQuery, IRequest<MemberStatementViewModel>
{
    public MemberStatementQuery() : base()
        => MinDate = DateTime.Today.AddMonths(-3).FirstDayOfMonth();

    public string ProductIds { get; set; } = "1,6,2";
}
