using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.Statement;

public class SMAStatementQuery : DefaultDateRangeQuery, IRequest<SMAStatementViewModel>
{
    public SMAStatementQuery() : base()
    {
        var today = SystemTime.Today;
        FromDate = SystemTime.Yesterday;
        MinDate = today.AddYears(-3).FirstDayOfYear();
        UserSelectedDateRangeOption = DateRangePickerViewModel.SpecificDateOptionValue;
    }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 50;
}
