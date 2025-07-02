namespace AgencyCash.API.RegisterCashMember.Responses;

public class CheckUsernameResult
{
    public bool Available { get; }

    public CheckUsernameResult(bool available)
    {
        Available = available;
    }
}
