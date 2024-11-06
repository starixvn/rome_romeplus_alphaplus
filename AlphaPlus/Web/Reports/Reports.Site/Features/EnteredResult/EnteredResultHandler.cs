using AlphaPlus.Reports.Features.EnteredResult;
using MediatR;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultHandler(
    IEnteredResultService enteredResultService,
    IEnteredResultWorkbook workbook) :
        IRequestHandler<EnteredResultQuery, EnteredResultViewModel>,
        IRequestHandler<EnteredResultExcelQuery, ExcelFileViewModel>
{
    private readonly IEnteredResultService enteredResultService = enteredResultService;
    private readonly IEnteredResultWorkbook workbook = workbook;

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
