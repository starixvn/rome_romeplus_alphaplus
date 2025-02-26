namespace AlphaPlus.Reports.Features.Customer;

public class CustomerLevel
{
    public int CustId { get; init; }

    public byte RoleId { get; init; }

    public string Username { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Username2 { get; init; }

    public int? MemberId { get; init; }

    public string MemberName { get; init; }

    public int? AgentId { get; init; }

    public string AgentName { get; init; }

    public int MasterId { get; init; }

    public string MasterName { get; init; }

    public int SuperId { get; init; }

    public string SuperName { get; init; }
}