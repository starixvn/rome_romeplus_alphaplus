using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class DailyWinLossHandler(IWhiteLabelService whiteLabelService, IDailyWinLossWorkbook workbook) :
    IRequestHandler<DailyWinLossQuery, DailyWinLossViewModel>,
    IRequestHandler<DailyWinLossGetDataQuery, DailyWinLossGetDataViewModel>,
    IRequestHandler<DailyWinLossExcelQuery, ExcelFileViewModel>
{
    private readonly IWhiteLabelService whiteLabelService = whiteLabelService;
    private readonly IDailyWinLossWorkbook workbook = workbook;

    public async Task<DailyWinLossViewModel> Handle(DailyWinLossQuery message, CancellationToken cancellationToken)
    {
        IEnumerable<IdNameModel> whiteLabels = [];
        if (message.ViewerLevel.IsNotAgentLevel())
        {
            whiteLabels = await whiteLabelService.GetWhiteLabels(message.ViewerId, message.Site);
            message.WhiteLabels = whiteLabels.Select(wl => wl.Id).JoinWithComma();
        }

        var winloss = await GetDailyWinLoss(message);

        return new DailyWinLossViewModel(message, whiteLabels, winloss);
    }

    public async Task<DailyWinLossGetDataViewModel> Handle(DailyWinLossGetDataQuery message, CancellationToken cancellationToken)
        => new DailyWinLossGetDataViewModel(message.ViewerLevel, await GetDailyWinLoss(message));

    public async Task<ExcelFileViewModel> Handle(DailyWinLossExcelQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetDailyWinLoss(message);

        return workbook.BuildExcelFile(message, reportData);
    }

    private Task<ReportData<DailyWinLossModel>> GetDailyWinLoss(DailyWinLossGetDataQuery message)
    {
        if (message.ViewerLevel.IsAgentLevel())
        {
            message.WhiteLabels = null;
            message.Username = null;
        }

        return whiteLabelService.GetDailyWinLoss(new WhiteLabelParam(
            message.ViewerId,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.WhiteLabels,
            message.Username,
            message.Site));
    }
}