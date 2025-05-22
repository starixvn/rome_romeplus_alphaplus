namespace AgencyCash.API.RegisterCashMember.Requests;

public class RegisterCashMemberRequest
{
    public required int AgentId { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Mobile { get; set; }

    public IEnumerable<string> Validate()
    {
        if (AgentId <= 0)
            yield return "AgentId must be greater than 0.";
        if (!Mobile.All(char.IsDigit))
            yield return "Mobile must be numeric only.";
        // ...add other validations as needed...
    }
}
