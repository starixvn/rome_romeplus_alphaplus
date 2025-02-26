using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BonusDetail;

public class ConfirmPaymentCommand : BaseQuery, IRequest<Result>
{
    public ConfirmPaymentCommand()
    {
        SubAccId = Viewer.SubAccId;
        IP = Viewer.UserLoginIp;
        LanguageKey = Viewer.LangKey;
        IsInternal = Viewer.IsInternal;
    }

    public long TransId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public int SubAccId { get; }

    public string IP { get; }

    public string LanguageKey { get; }

    public bool IsInternal { get; }

    public override bool IsValid() => !IsInternal && base.IsValid();
}