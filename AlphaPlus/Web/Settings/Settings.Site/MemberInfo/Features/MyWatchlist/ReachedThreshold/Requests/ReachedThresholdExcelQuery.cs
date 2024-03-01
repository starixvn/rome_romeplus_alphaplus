namespace Alpha.MemberInfo.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using Alpha.MemberInfo.Site.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ReachedThresholdExcelQuery : ReachedThresholdQueryBase, IRequest<ExcelFileViewModel>
    {
        public ReachedThresholdExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}