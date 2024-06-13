namespace AlphaPlus.BetLists.Shared.Models;

public class ProductDetail
{
    public string RefId { get; init; }

    public int ResourceId { get; init; }

    public int ProductId { get; init; }

    public int SportType { get; init; }

    public int BetType { get; init; }

    public int Order { get; init; }

    public string DisplayName { get; set; }

    public string LeagueGroupId { get; init; }

    public string[] LeagueGroupIds => LeagueGroupId.ToList();

    public string TicketType { get; init; }

    public string[] TicketTypeIds => TicketType.ToList();
}