using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.Shared.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingHandler(
    IIPMatchingService ipMatchingService,
    IIPMatchingWorkbook workbook) :
        IRequestHandler<IPMatchingQuery, IPMatchingViewModel>,
        IRequestHandler<IPMatchingExcelQuery, ExcelFileViewModel>
{
    public async Task<IPMatchingViewModel> Handle(IPMatchingQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetIPMatching(message);

        return new IPMatchingViewModel(message, reportData);
    }

    public async Task<ExcelFileViewModel> Handle(IPMatchingExcelQuery message, CancellationToken cancellationToken)
    {
        var reportData = await GetIPMatching(message);

        return workbook.BuildExcelFile(message, reportData);
    }

    private Task<ReportData<IPMatchingModel>> GetIPMatching(IPMatchingQuery message)
    {
        var param = new IPMatchingParam(
            message.ViewerId,
            message.Date,
            message.MemberCount,
            message.IsAdminCanViewVNIP,
            message.Site);

        return ipMatchingService.GetIPMatching(param);
    }
}
