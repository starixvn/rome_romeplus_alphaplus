namespace Alpha.Tickets.Infrastructure.Models.IPRange;

public record IPRangeV6
{
    public string IP { get; init; }

    public byte[] IPFrom { get; init; }

    public byte[] IPTo { get; init; }

    public string CountryShort { get; init; }

    public string CountryLong { get; init; }
}
