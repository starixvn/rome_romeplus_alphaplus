namespace AlphaPlus.Reports.Site.Features.Legends;

internal static class LegendProductIdConstants
{
    private const int Allbet = 21;
    private const int SAGaming = 39;
    private const int SabaClub = 42;
    private const int SexyGaming = 45;
    private const int GTCasino = 46;
    private const int BBin = 47;
    private const int RNGWar = 50;
    private const int WM = 51;
    private const int ION = 52;
    public const int DummyRNGProductId = 53;
    private const int BigGame = 57;    
    private const int PPLiveCasino = 65;
    private const int VGaming = 66;
    private const int FGG = 70;
    private const int YeeBet = 76;
    private const int WE = 80;
    private const int ONCasino = 96;

    public static HashSet<int> LiveCasinoProductIds { get; } = new HashSet<int>
    {
        Allbet,
        SexyGaming,
        SAGaming,
        GTCasino,
        BBin,
        ION,
        WM,
        BigGame,
        PPLiveCasino,
        YeeBet,
        FGG,
        WE,
        ONCasino
    };

    public static HashSet<int> RNGCasinoProductIds { get; } = new HashSet<int>
    {
        RNGWar,
        SabaClub,
        VGaming
    };
}