using AlphaPlus.Core;
using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.RemarkInfo.Update
{
    public class UpdateRemarkInfoQuery : QueryBase, IRequest<Result>
    {
        public int SearchAccountId { get; set; }
        public string Remark { get; set; }
    }
}