using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments.Models;

public record PaymentResult(int Code, string Message, long TransId) : Result(Code, Message);