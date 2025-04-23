using AlphaPlus.Core.Extensions;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class SettlementHandler(IWhiteLabelService whiteLabelService, ISettlementWorkbook workbook) :
    IRequestHandler<SettlementQuery, SettlementViewModel>,
    IRequestHandler<SettlementGetDataQuery, SettlementGetDataViewModel>,
    IRequestHandler<SettlementExcelQuery, ExcelFileViewModel>
{
    private readonly IWhiteLabelService whiteLabelService = whiteLabelService;
    private readonly ISettlementWorkbook workbook = workbook;

    public async Task<SettlementViewModel> Handle(SettlementQuery message, CancellationToken cancellationToken)
    {
        IEnumerable<IdNameModel> whiteLabels = [];
        if (message.ViewerLevel.IsNotAgentLevel())
        {
            whiteLabels = await whiteLabelService.GetWhiteLabels(message.ViewerId, message.Site);
            if (string.IsNullOrWhiteSpace(message.WhiteLabels))
            {
                message.WhiteLabels = whiteLabels.Select(wl => wl.Id).JoinWithComma();
            }
        }

        var settlement = await GetSettlement(message);

        return new SettlementViewModel(message, whiteLabels, settlement);
    }

    public async Task<SettlementGetDataViewModel> Handle(SettlementGetDataQuery message, CancellationToken cancellationToken)
        => new SettlementGetDataViewModel(message.ViewerLevel, await GetSettlement(message));

    public async Task<ExcelFileViewModel> Handle(SettlementExcelQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetSettlement(message);

        return workbook.BuildExcelFile(message, reportData);
    }

    private Task<ReportData<SettlementModel>> GetSettlement(SettlementGetDataQuery message)
    {
        if (message.ViewerLevel.IsAgentLevel())
        {
            message.WhiteLabels = null;
            message.Username = null;
        }

        return whiteLabelService.GetSettlement(new WhiteLabelParam(
            message.ViewerId,
            message.FromDate.FirstDayOfMonth(),
            message.ToDate.LastDayOfMonth(),
            message.WhiteLabels,
            message.Username,
            message.Site));
    }
}
