using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class StatementDetailsQuery : BaseQuery, IRequest<StatementDetailViewModel>
{
    public StatementDetailsQuery() : base()
        => MinDate = DateTime.Today.AddYears(-3).FirstDayOfYear();

    public DateTime TransferDate { get; set; }

    public override bool IsValid() => base.IsValid() && TransferDate >= MinDate;
}
