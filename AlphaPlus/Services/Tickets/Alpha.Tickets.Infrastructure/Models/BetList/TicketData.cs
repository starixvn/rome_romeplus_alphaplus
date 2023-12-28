namespace Alpha.Tickets.Infrastructure.Models.BetList;

public record TicketData
{
    public string RefNo { get; init; }
    public int BetTypeId { get; init; }
    public string Status { get; init; }
    public decimal Odds { get; init; }
    public decimal Stake { get; init; }
    public string BetTeam { get; init; }
    public string TransDesc { get; init; }
    public decimal SuperPT { get; init; }
    public decimal MasterPT { get; init; }
    public decimal AgentPT { get; init; }
}
