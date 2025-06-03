using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;

public class MemberBalanceInfoQuery : QueryBase, IRequest<IBalanceInfoViewModel>
{
    public string CustName { get; set; }

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CustName);
}