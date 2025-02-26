namespace AlphaPlus.Reports.Features.BonusDetail;

public record ConfirmPaymentParam(
    int ViewerId,
    int SubAccId,
    int CustId,
    long TransId,
    DateTime PaymentDate,
    string LoginIP,
    string Language);
