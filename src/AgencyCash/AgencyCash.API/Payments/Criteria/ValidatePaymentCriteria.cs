using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "<Pending>")]
public class ValidatePaymentCriteria(
    int agentId,
    int custId,
    byte transType,
    short custDBGroupId,
    string paymentFrom = null,
    bool isManual = false,
    short providerId = 0,
    short paymentMethodId = 0,
    string paymentTo = null) : NonQueryCommand
{
    public int AgentId { get; } = agentId;

    public int CustId { get; } = custId;

    public byte TransType { get; } = transType;

    public string PaymentFrom { get; set; } = paymentFrom;

    public bool IsManual { get; set; } = isManual;

    public short ProviderId { get; set; } = providerId;

    public short PaymentMethodId { get; set; } = paymentMethodId;

    public string PaymentTo { get; set; } = paymentTo;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public string ErrExtraInfo { get; set; }

    public string Username2 { get; set; } = string.Empty;

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("API_CashService_CustPaymentValidate", custDBGroupId);

    public override bool IsValid() => AgentId > 0 && CustId > 0;
}