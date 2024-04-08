using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultQuery : BaseQuery, IRequest<EnteredResultViewModel>
{
    public EnteredResultQuery() : base()
        => MinDate = DateTime.Today.AddMonths(-3).FirstDayOfMonth();
}
