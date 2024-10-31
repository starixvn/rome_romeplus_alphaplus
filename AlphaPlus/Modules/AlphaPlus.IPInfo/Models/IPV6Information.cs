namespace AlphaPlus.IPInfo.Models;

public class IPV6Information
{
    public string IP { get; init; }

    public byte[] IPFrom { get; init; }

    public byte[] IPTo { get; init; }

    public string CountryShort { get; init; }

    public string CountryLong { get; init; }
}
