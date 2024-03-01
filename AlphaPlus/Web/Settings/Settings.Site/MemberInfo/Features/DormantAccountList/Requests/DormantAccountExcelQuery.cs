namespace Alpha.MemberInfo.Site.Features.DormantAccountList.Requests
{
    using Alpha.MemberInfo.Site.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class DormantAccountExcelQuery : DormantAccountQuery, IRequest<ExcelFileViewModel>
    {
        public DormantAccountExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}