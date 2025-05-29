namespace AgencyCash.API.Payments.Models;

public record DepositLimit(decimal MinAmount, decimal MaxAmount, decimal AgentAvailableCredit);
