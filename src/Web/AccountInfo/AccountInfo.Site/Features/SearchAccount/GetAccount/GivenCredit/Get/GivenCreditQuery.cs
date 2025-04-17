using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Get;

public class GivenCreditQuery : QueryBase, IRequest<GivenCreditViewModel>
{
    public string SearchAccountName { get; set; }

    public byte SearchAccountRoleId { get; set; }

    public virtual int SearchAccountId { get; set; }
}