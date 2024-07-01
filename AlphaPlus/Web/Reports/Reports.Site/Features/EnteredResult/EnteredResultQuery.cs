using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultQuery : BaseQuery, IRequest<EnteredResultViewModel>
{
    public EnteredResultQuery() : base()
        => MinDate = SystemTime.Today.AddMonths(-3).FirstDayOfMonth();
}
