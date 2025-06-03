using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.GivenCredit.Update;

public class GivenCreditCommand : QueryBase, IRequest<Result>
{
    public virtual int SearchAccountId { get; set; }

    public byte SearchAccountRoleId { get; set; }

    public decimal CreditAmount { get; set; }

    public override bool IsValid() =>
        base.IsValid() &&
        CanUpdateAccount &&
        !IsInternal &&
        SearchAccountRoleId < ViewerId;
}