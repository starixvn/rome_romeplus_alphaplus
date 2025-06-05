using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments.Models;

public record WithdrawalLimitResult(decimal MinAmount = 0, decimal MaxAmount = 0, int DailyLimit = 0, decimal MemberCashBalance = 0);