using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;
using MediatR;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class BetCreditHistoryHandler(
    IBetCreditHistoryService betCreditHistoryService,
    IBetCreditHistoryWorkbook workbook) :
        IRequestHandler<BetCreditHistoryQuery, BetCreditHistoryViewModel>,
        IRequestHandler<BetCreditHistoryExcelQuery, ExcelFileViewModel>
{
    public async Task<BetCreditHistoryViewModel> Handle(BetCreditHistoryQuery message, CancellationToken cancellationToken)
    {
        var model = await GetBetCreditHistory(message);

        return new BetCreditHistoryViewModel(message, model);
    }

    public async Task<ExcelFileViewModel> Handle(BetCreditHistoryExcelQuery message, CancellationToken cancellationToken)
    {
        var model = await GetBetCreditHistory(message);

        return workbook.BuildExcelFile(message, model.ReportData);
    }

    private async Task<BetCreditHistoryModel> GetBetCreditHistory(BetCreditHistoryQuery message)
    {
        var param = new BetCreditHistoryParam(
            message.Member,
            message.RefNo,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.ViewerId,
            message.Language,
            message.PageIndex,
            message.PageSize,
            message.Site);

        var model = await betCreditHistoryService.GetBetCreditHistory(param);
        model.ErrorMessage = model.ErrorCode == 101 ? Resources.InvalidUsername : model.ErrorMessage;

        return model;
    }
}
