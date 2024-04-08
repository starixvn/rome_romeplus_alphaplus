using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WinLossDetail;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLossDetail;

public class WinLossDetailGetDataQuery : WinLossDetailNewQuery, IRequest<ReportData<WinLossDetailModel>>
{

}