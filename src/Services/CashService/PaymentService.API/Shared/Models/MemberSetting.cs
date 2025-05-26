namespace PaymentService.API.Payments.Models;

public record MemberSetting(
    decimal AgentGivenCredit,
    decimal CreditMembersGivenCredit,
    decimal MinLimitAmount,
    decimal MaxLimitAmount);