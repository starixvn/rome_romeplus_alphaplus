namespace AlphaPlus.IPInfo.Models;

public class IPV6Information
{
    public string IP { get; set; }

    public byte[] IPFrom { get; set; }

    public byte[] IPTo { get; set; }

    public string CountryShort { get; set; }

    public string CountryLong { get; set; }
}
