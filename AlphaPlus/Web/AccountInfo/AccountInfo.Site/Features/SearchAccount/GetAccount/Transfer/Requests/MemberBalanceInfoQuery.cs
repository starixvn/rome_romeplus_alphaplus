using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.Transfer.Requests;

public class MemberBalanceInfoQuery : BaseQuery, IRequest<IBalanceInfoViewModel>
{
    public string CustName { get; set; }

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CustName);
}