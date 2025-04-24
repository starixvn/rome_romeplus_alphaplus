using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class MoveStatementHistoryHandler(
    IMoveStatementHistoryService moveStatementHistoryService,
    IMoveStatementHistoryWorkbook workbook) :
        IRequestHandler<MoveStatementHistoryQuery, MoveStatementHistoryViewModel>,
        IRequestHandler<MoveStatementHistoryExcelQuery, ExcelFileViewModel>
{
    private readonly IMoveStatementHistoryService moveStatementHistoryService = moveStatementHistoryService;
    private readonly IMoveStatementHistoryWorkbook workbook = workbook;

    public async Task<MoveStatementHistoryViewModel> Handle(MoveStatementHistoryQuery message, CancellationToken cancellationToken)
        => new MoveStatementHistoryViewModel(message, await GetMoveStatementHistory(message));

    public async Task<ExcelFileViewModel> Handle(MoveStatementHistoryExcelQuery message, CancellationToken cancellationToken)
    {
        var model = await GetMoveStatementHistory(message);

        return workbook.BuildExcelFile(message, model.ReportData);
    }

    private Task<MoveStatementHistoryModel> GetMoveStatementHistory(MoveStatementHistoryQuery message)
    {
        var param = new MoveStatementHistoryParam(
            message.Member,
            message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
            message.ToDate,
            message.ViewerId,
            message.PageIndex,
            message.PageSize,
            !message.IsMainReport,
            message.Site);

        return moveStatementHistoryService.GetMoveStatementHistory(param);
    }
}