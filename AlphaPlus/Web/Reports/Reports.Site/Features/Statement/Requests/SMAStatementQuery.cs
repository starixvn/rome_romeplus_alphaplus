using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class SMAStatementQuery : DefaultDateRangeQuery, IRequest<SMAStatementViewModel>
{
    public SMAStatementQuery() : base()
    {
        var today = DateTime.Today;
        FromDate = today.Yesterday();
        MinDate = today.FirstDayOf3YearsAgo();
        UserSelectedDateRangeOption = DateRangePickerViewModel.SpecificDateOptionValue;
    }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 50;
}
