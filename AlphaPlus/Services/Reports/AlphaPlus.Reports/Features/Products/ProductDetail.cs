namespace AlphaPlus.Reports.Features.Products;

public class ProductDetail
{
    public string RefId { get; init; }

    public int ResourceId { get; init; }

    public int ProductId { get; init; }

    public int SportType { get; init; }

    public int BetType { get; init; }

    public int Order { get; init; }

    public string DisplayName { get; set; }

    public int LeagueGroupId { get; init; }

    public int TicketType { get; init; }

    public int ChildResourceId { get; init; }

    public string ToXml() => string.Format(
        "<ProductDetail ProductId=\"{0}\" Sporttype=\"{1}\" Bettype=\"{2}\" LeagueGroupId=\"{3}\" TicketType=\"{4}\"/>",
        ProductId,
        SportType,
        BetType,
        LeagueGroupId,
        TicketType);
}