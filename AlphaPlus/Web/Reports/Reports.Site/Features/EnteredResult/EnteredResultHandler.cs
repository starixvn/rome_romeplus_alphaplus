using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;
using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultHandler(
    IEnteredResultService enteredResultService,
    IEnteredResultWorkbook workbook) :
        IRequestHandler<EnteredResultQuery, EnteredResultViewModel>,
        IRequestHandler<EnteredResultExcelQuery, ExcelFileViewModel>
{
    public async Task<EnteredResultViewModel> Handle(EnteredResultQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetEnteredResults(message.Site);

        return new EnteredResultViewModel(message, reportData);
    }

    public async Task<ExcelFileViewModel> Handle(EnteredResultExcelQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetEnteredResults(message.Site);

        return workbook.BuildExcelFile(message, reportData);
    }

    private async Task<ReportData<EnteredResultSport>> GetEnteredResults(string siteName)
    {
        var reportData = await enteredResultService.GetEnteredResults(siteName);
        var total = enteredResultService.GetTotalEnteredResults(reportData ?? []);

        return new ReportData<EnteredResultSport>(reportData, total);
    }
}
