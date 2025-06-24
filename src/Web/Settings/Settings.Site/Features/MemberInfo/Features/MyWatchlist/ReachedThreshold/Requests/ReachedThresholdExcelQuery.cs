using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachedThresholdExcelQuery : ReachedThresholdQueryBase, IRequest<ExcelFileViewModel>
    {
    }
}