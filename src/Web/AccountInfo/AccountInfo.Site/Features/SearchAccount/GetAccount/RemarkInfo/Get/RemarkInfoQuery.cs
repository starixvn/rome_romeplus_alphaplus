using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.RemarkInfo.Get;

public class RemarkInfoQuery : QueryBase, IRequest<RemarkInfoViewModel>
{
    public string SearchAccountName { get; set; }

    public byte SearchAccountRoleId { get; set; }

    public virtual int SearchAccountId { get; set; }

    public int ActorId => userSession.CustId;
}