
namespace AlphaPlus.Reports.Features.Shared.Constants;

public static class ProductDetaiIdConstants
{
    public const int Sportsbook = 11;
    public const int Soccer = 12;
    public const int Basketball = 13;
    public const int Cricket = 14;
    public const int Tennis = 20;

    public static readonly HashSet<int> SportsbookDetailFor777Site = new HashSet<int>
    {
        Sportsbook,
        Soccer,
        Basketball,
        Cricket,
        Tennis
    };
}