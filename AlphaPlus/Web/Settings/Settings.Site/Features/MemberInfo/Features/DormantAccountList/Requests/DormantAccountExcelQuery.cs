using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.Requests
{
    public class DormantAccountExcelQuery : DormantAccountQuery, IRequest<ExcelFileViewModel>
    {
        public DormantAccountExcelQuery() : this(null) { }

        public DormantAccountExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}