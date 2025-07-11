namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;

using Fanex.Data.Repository;

public class GetAgentPaymentGatewayInfoCriteria : CriteriaBase
{
    public int AgentId { get; set; }
    public int TransactionType { get; set; }
    public int ProviderId { get; set; }
    public int PaymentMethodId { get; set; }

    public override string GetSettingKey() => "AgentPaymentGatewayInfo";

    public override bool IsValid() => AgentId > 0 && TransactionType > 0 && ProviderId > 0 && PaymentMethodId > 0;
}