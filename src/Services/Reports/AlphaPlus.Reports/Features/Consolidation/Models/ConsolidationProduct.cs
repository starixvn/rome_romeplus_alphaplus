namespace AlphaPlus.Reports.Features.Consolidation;

public class ConsolidationProduct
{
    public int ProductId { get; init; }

    public string ProductName { get; set; }

    public DateTime? EndOfSupportDate { get; init; }

    public bool IsShow { get; init; }

    public int OrderNo { get; init; }

    public int Type { get; init; }

    public string TypeName { get; set; }
}