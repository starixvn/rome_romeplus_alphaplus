namespace Alpha.Tickets.Infrastructure.Models.IPRange;

public record IPRangeV4
{
    public string IP { get; init; }

    public long IPFrom { get; init; }

    public long IPTo { get; init; }

    public string CountryShort { get; init; }

    public string CountryLong { get; init; }
}
