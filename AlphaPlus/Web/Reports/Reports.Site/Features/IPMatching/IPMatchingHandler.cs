using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Utilities.IP;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingHandler(
    IIPMatchingService ipMatchingService,
    IPService ipService,
    IIPMatchingWorkbook workbook) :
        IRequestHandler<IPMatchingQuery, IPMatchingViewModel>,
        IRequestHandler<IPMatchingExcelQuery, ExcelFileViewModel>
{
    public async Task<IPMatchingViewModel> Handle(IPMatchingQuery message, CancellationToken cancellationToken)
    {
        var (reportData, total) = await GetIPMatching(message);

        return new IPMatchingViewModel(message, reportData, total);
    }

    public async Task<ExcelFileViewModel> Handle(IPMatchingExcelQuery message, CancellationToken cancellationToken)
    {
        var (reportData, total) = await GetIPMatching(message);

        return workbook.BuildExcelFile(message, reportData, total);
    }

    private async Task<(IEnumerable<IPMatchingModel>, IPMatchingModel)> GetIPMatching(IPMatchingQuery message)
    {
        var param = new IPMatchingParam(message.CustId, message.Date, message.MemberCount, message.Site);
        var rawdata = await ipMatchingService.GetIPMatching(param);

        var filteredRawData = new List<IPMatchingModel>();
        var total = new IPMatchingModel { IP = Resources.Total };

        foreach (var item in rawdata)
        {
            if (message.IsAdminCanViewVNIP || !ipService.IsVNIP(item.IP))
            {
                filteredRawData.Add(item);
                total += item;
            }
        }

        return (filteredRawData, total);
    }
}
