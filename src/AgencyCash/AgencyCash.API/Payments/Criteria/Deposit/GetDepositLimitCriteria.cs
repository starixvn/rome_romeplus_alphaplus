using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria.Deposit;

public class GetDepositLimitCriteria(
    int custId,
    byte paymentType,
    short providerId,
    short paymentMethodId,
    decimal accumCreditReturn,
    short custDBGroupId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public byte PaymentType { get; } = paymentType;

    public short ProviderId { get; } = providerId;

    public short PaymentMethodId { get; } = paymentMethodId;

    public decimal AccumCreditReturn { get; } = accumCreditReturn;

    public bool IsMemberSite { get; } = true;

    public decimal MinAmt { get; set; }

    public decimal MaxAmt { get; set; }

    public decimal AgentAvailableCredit { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("Age_CashDeposit_LimitAmtSel", custDBGroupId);

    public override bool IsValid() => CustId > 0;
}