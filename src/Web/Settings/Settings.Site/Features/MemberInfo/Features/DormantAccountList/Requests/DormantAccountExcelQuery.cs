using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.DormantAccountList.Requests
{
    public class DormantAccountExcelQuery : DormantAccountQuery, IRequest<ExcelFileViewModel>
    {
    }
}