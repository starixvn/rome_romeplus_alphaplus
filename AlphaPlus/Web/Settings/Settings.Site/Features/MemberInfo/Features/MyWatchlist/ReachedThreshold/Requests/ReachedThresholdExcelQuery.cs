namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using AlphaPlus.Core.Models;

    public class ReachedThresholdExcelQuery : ReachedThresholdQueryBase, IRequest<ExcelFileViewModel>
    {
        public ReachedThresholdExcelQuery() : this(null) { }

        public ReachedThresholdExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}