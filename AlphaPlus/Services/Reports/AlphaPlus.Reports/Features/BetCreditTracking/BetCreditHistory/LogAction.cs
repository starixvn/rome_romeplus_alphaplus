namespace AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

public class LogAction
{
    public int Id { get; init; }

    public int ActionTypeId { get; init; }

    public string ChineseTraditionalName { get; init; }

    public string ChineseSimplifiedName { get; init; }

    public string EnglishName { get; init; }

    public string VietnameseName { get; init; }

    public string JapaneseName { get; init; }

    public string KoreanName { get; init; }

    public string ThaiName { get; init; }

    public bool IsInternal { get; init; }

    public string SPName { get; init; }
}