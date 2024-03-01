using AlphaPlus.Core.Helpers;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.EnteredResult;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.EnteredResult;

public class EnteredResultHandler(
    IEnteredResultService enteredResultService,
    IEnteredResultWorkbook workbook) :
        IRequestHandler<EnteredResultQuery, EnteredResultViewModel>,
        IRequestHandler<EnteredResultExcelQuery, ExcelFileViewModel>
{
    public async Task<EnteredResultViewModel> Handle(EnteredResultQuery message, CancellationToken cancellationToken)
    {
        var (reportData, totalEnteredResults) = await GetEnteredResults(message.Site);

        return new EnteredResultViewModel(message, reportData, totalEnteredResults);
    }

    public async Task<ExcelFileViewModel> Handle(EnteredResultExcelQuery message, CancellationToken cancellationToken)
    {
        var (reportData, totalEnteredResults) = await GetEnteredResults(message.Site);

        var today = DateTime.Today;
        var resources = new Dictionary<string, string>
        {
            ["Yesterday"] = today.Yesterday().ToShortDateWithSlash(),
            ["Today"] = today.ToShortDateWithSlash()
        };

        var result = workbook
            .SetRowMapper()
            .SetData(reportData, totalEnteredResults)
            .UseTemplate(workbook.GetTemplateInfo())
            .Header(key => resources.TryGetValue(key, out var value) ? value : Resources.ResourceManager.GetString(key))
            .Sheet(Resources.DefaultSheetName)
            .ExportToBytes();

        return new ExcelFileViewModel(workbook.GetFileName(resources), result);
    }

    private async Task<(IEnumerable<EnteredResultSport>, EnteredResultSport)> GetEnteredResults(string siteName)
    {
        var reportData = await enteredResultService.GetEnteredResults(siteName);
        var total = enteredResultService.GetTotalEnteredResults(reportData ?? []);

        return (reportData, total);
    }
}
