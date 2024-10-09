using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Transfer.Requests;

public class UpdateAllDayBalanceTransferRequest : BaseQuery, IRequest<Result>
{
    private readonly IUserSession viewer;

    public UpdateAllDayBalanceTransferRequest(IUserSession viewer) : base(viewer)
    {
        this.viewer = viewer;
    }

    public bool StatusValue { get; set; }

    public int ActorId => viewer.CustId;

    public string Language => viewer.LangKey;

    public string Currency => viewer.Currency;

    public override bool IsValid()
    {
        var isValid = base.IsValid() && CanUpdateAccount && !IsInternal;
        return isValid;
    }
}