namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using AlphaPlus.Settings.Site.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ReachedThresholdExcelQuery : ReachedThresholdQueryBase, IRequest<ExcelFileViewModel>
    {
        public ReachedThresholdExcelQuery() : this(null){}
        
        public ReachedThresholdExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}