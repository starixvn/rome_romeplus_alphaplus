namespace AgencyCash.API.RegisterCashMember.Responses;

public class CheckUsernameResult(bool available)
{
    public bool Available { get; } = available;
}
