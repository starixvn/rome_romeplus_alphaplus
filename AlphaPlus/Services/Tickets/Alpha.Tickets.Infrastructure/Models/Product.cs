namespace Alpha.Tickets.Infrastructure.Models;

public record Product
{
    public int ProductId { get; init; }

    public string ProductName { get; init; }

    public bool IsHidden { get; init; }

    public DateTime? EndOfSupportDate { get; init; }

    public int OrderNo { get; init; }

    public int Type { get; init; }
}
