namespace Alpha.Tickets.Infrastructure.Models;

internal record FollowBetAccount
{
    public int CustId { get; init; }

    public string Username { get; init; }

    public int AgentId { get; init; }

    public int DummyCustId { get; init; }

    public string DummyUsername { get; init; }
}
