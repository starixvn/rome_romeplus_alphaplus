using AlphaPlus.Reports.Features.BetChoiceMatching;

namespace AlphaPlus.Reports.Site.Features.BetChoiceMatching;

public class BetChoiceMatchingJsonModel(
    IEnumerable<BetChoiceMatchingEventModel> events,
    IEnumerable<KeyValuePair<int, string>> teams,
    IEnumerable<KeyValuePair<int, string>> leagues,
    IEnumerable<KeyValuePair<int, string>> sportTypes,
    IEnumerable<KeyValuePair<int, string>> betTypes,
    IEnumerable<BetChoiceMatchingSharedIPModel> ips)
{
    public IEnumerable<BetChoiceMatchingEventModel> Events { get; } = events;

    public IEnumerable<KeyValuePair<int, string>> Teams { get; } = teams;

    public IEnumerable<KeyValuePair<int, string>> Leagues { get; } = leagues;

    public IEnumerable<KeyValuePair<int, string>> SportTypes { get; } = sportTypes;

    public IEnumerable<KeyValuePair<int, string>> BetTypes { get; } = betTypes;

    public IEnumerable<BetChoiceMatchingSharedIPModel> IPs { get; set; } = ips;
}