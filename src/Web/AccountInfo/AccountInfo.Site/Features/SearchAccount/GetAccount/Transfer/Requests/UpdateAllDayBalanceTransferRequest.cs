using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Transfer.Requests;

public class UpdateAllDayBalanceTransferRequest : QueryBase, IRequest<Result>
{
    public bool StatusValue { get; set; }

    public int ActorId => userSession.CustId;

    public string Currency => userSession.Currency;

    public int SearchAccountId { get; set; }

    public override bool IsValid()
    {
        var isValid = base.IsValid() && CanUpdateAccount && !IsInternal;
        return isValid;
    }
}