using AlphaPlus.Reports.Features.Shared.Extensions;

namespace AlphaPlus.Reports.Features.BonusDetail;

public class BonusEntity
{
    public DateTime CreatedDate { get; init; }

    public long TransId { get; init; }

    public string MasterName { get; init; }

    public string AgentName { get; init; }

    public int CustId { get; init; }

    public string Username { get; init; }

    public decimal Amount { get; init; }

    public DateTime? PaymentDate { get; init; }

    public byte StatusId { get; init; }

    public bool IsComplained { get; init; }

    public string CreatedDateString => CreatedDate.ToLongDateWithoutSecondWithSlash();

    public string PaymentDateString
        => PaymentDate.HasValue ? PaymentDate.Value.ToShortDateWithSlash() : string.Empty;

    public string MinConfirmPaymentDate => CreatedDate.ToShortDateWithSlash();

    public bool IsCancelled => StatusId == BonusStatusIdConst.Cancelled;

    public bool CanConfirmPaymentDate => StatusId == BonusStatusIdConst.Pending;
}