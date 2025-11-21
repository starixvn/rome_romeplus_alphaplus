using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.Shared.Models;
using MediatR;
using AlphaPlus.Utilities.IP;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingHandler(
    IIPMatchingService ipMatchingService,
    IIPMatchingWorkbook workbook,
    IPService ipService) :
        IRequestHandler<IPMatchingQuery, IPMatchingViewModel>,
        IRequestHandler<IPMatchingExcelQuery, ExcelFileViewModel>
{
    private readonly IIPMatchingService ipMatchingService = ipMatchingService;
    private readonly IIPMatchingWorkbook workbook = workbook;
    private readonly IPService ipService = ipService;
    public async Task<IPMatchingViewModel> Handle(IPMatchingQuery message, CancellationToken cancellationToken)
    {
        if (await CheckHideIP(message.SRecommendId, message.System))
        {
            throw new UnauthorizedAccessException(CommonResx.NotAuthorized);
        }

        var reportData = await GetIPMatching(message);

        return new IPMatchingViewModel(message, reportData);
    }

    public async Task<ExcelFileViewModel> Handle(IPMatchingExcelQuery message, CancellationToken cancellationToken)
    {
        if (await CheckHideIP(message.SRecommendId, message.System))
        {
            throw new UnauthorizedAccessException(CommonResx.NotAuthorized);
        }   
        
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

    private async Task<bool> CheckHideIP(int srecommendId, string system) 
        => await ipService.CheckHideIP(srecommendId, system);
}
