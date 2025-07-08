namespace AlphaPlus.CashService.Site.Features.Promotion.Enum;

using System.ComponentModel;

public enum ProductCategory
{
    [Description("Sportsbooks")]
    Sportsbooks = 1,

    [Description("RNG Games")]
    RngGames = 2,

    [Description("RNG Casino")]
    RngCasino = 3,

    [Description("Live Casino")]
    LiveCasino = 4,
}