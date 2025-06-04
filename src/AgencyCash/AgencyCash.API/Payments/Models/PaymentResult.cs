using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments.Models;

public record PaymentResult(int Code, string Message, long TransId = 0) : Result(Code, Message);