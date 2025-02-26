namespace AlphaPlus.BetLists.LastBets;

public class FilterConfiguration
{
    public int FilterId { get; init; }

    public string FilterName { get; init; }

    public string FilterValue { get; init; }

    public FilterXml FilterXml { get; set; }

    public bool IsDefault { get; init; }

    public bool IsNotify { get; set; }

    public DateTime CreatedDate { get; init; }

    public string WatchList { get; init; }

    public string InputList { get; init; }
}
