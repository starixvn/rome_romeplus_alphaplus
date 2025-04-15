using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BonusDetail;

public class ConfirmPaymentCriteria(
    int custId,
    int subAccId,
    long transId,
    DateTime paymentDate,
    int userId,
    string loginIP,
    string language) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public int SubAccId { get; } = subAccId;

    public long TransId { get; } = transId;

    public DateTime PaymentDate { get; } = paymentDate;

    public int UserId { get; } = userId;

    public string LoginIP { get; } = loginIP;

    public string Language { get; } = language;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey() => "Age_Bonus_ConfirmPayment";

    public override bool IsValid() => CustId > 0 && TransId > 0 && UserId > 0;
}
