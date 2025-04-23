namespace AlphaPlus.Reports.Site.Features.Legends;

public record LegendViewModel(IEnumerable<string> Legends, bool MobileFriendly = false);
