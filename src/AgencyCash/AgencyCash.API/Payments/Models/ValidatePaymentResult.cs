using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments.Models;

public record ValidatePaymentResult(
    int Code,
    string Message,
    string MemberNickname) : Result(Code, Message);
