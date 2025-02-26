using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal class RewardConfirmPaymentLogCriteria(string paymentLog) : NonQueryCommand
{
    public string PaymentLog { get; } = paymentLog;

    public override string GetSettingKey() => "WriteLog_UserLog_RunSP_RewardPayment";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(PaymentLog);
}