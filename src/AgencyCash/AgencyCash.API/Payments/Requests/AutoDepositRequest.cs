using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Payments.Constants;

namespace AgencyCash.API.Payments.Requests;

public class AutoDepositRequest
{
    [Range(minimum: 1, maximum: 2, ErrorMessage = "Deposit Method must be 1 (Payment Gateway) or 2 (Decimal or Bank Account).")]
    public byte DepositMethod { get; init; }

    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = $"Agent ID must be in range [1, 2,147,483,647].")]
    public int AgentId { get; init; }

    [Range(minimum: 1, maximum: int.MaxValue)]
    public int CustId { get; init; }

    public short ProviderId { get; init; }

    [Range(minimum: 1, maximum: short.MaxValue)]
    public short PaymentMethodId { get; init; }

    public byte MachingMode { get; init; }

    public string PaymentFrom { get; init; }

    public string PaymentTo { get; init; }

    [Range(minimum: 0, maximum: double.MaxValue, MinimumIsExclusive = true)]
    public decimal Amount { get; init; }

    public string Remark { get; init; }

    public string IP { get; init; }

    public (bool, string) IsValid()
    {
        if (DepositMethod == DepositMethodConst.AutoPaymentGateway)
        {
            return IsValidPaymentGateway();
        }

        if (DepositMethod == DepositMethodConst.AutoDecimalOrBankAccount)
        {
            return IsValidDecimalOrBankAccount();
        }

        return (false, "Deposit Method must be 1 (Payment Gateway) or 2 (Decimal or Bank Account).");
    }

    private (bool, string) IsValidPaymentGateway()
    {
        if (ProviderId <= 0 || ProviderId > short.MaxValue)
        {
            return (false, $"Provider ID must be in range [1, {short.MaxValue}].");
        }

        return (true, string.Empty);
    }

    private (bool, string) IsValidDecimalOrBankAccount()
    {
        if (MachingMode != 1 && MachingMode != 2)
        {
            return (false, "Maching Mode must be 1 or 2.");
        }

        if (string.IsNullOrWhiteSpace(PaymentFrom))
        {
            return (false, "Payment From is required.");
        }
        else if (PaymentFrom.Length > 100)
        {
            return (false, "Maximum Payment From length is 100 characters long.");
        }

        if (string.IsNullOrWhiteSpace(PaymentTo))
        {
            return (false, "Payment To is required.");
        }
        else if (PaymentTo.Length > 100)
        {
            return (false, "Maximum Payment To length is 100 characters long.");
        }

        return (true, string.Empty);
    }
}