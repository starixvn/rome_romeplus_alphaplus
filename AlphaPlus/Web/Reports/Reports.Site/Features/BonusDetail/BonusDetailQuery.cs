using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Utils;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BonusDetail;

public class BonusDetailQuery : DefaultDateRangeQuery, IRequest<BonusDetailViewModel>
{
    public BonusDetailQuery() : base()
    {
        FromDate = SystemTime.Today.AddMonths(-1).FirstDayOfMonth();
        MinDate = new DateTime(2021, 1, 1);
        UserSelectedDateRangeOption = DateRangePickerViewModel.SinceLastMonthOptionValue;
    }

    public string Member { get; set; }

    public byte? StatusId { get; set; }

    public bool? IsComplained { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 500;
}