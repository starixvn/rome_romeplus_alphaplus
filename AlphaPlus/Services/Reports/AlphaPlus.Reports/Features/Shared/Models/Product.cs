namespace AlphaPlus.Reports.Features.Shared.Models;

public class Product
{
    public int Id { get; init; }

    public string Name { get; set; }

    public DateTime? EndOfSupportDate { get; init; }

    public int Type { get; init; }

    public string TypeName { get; set; }

    public int OrderNo { get; init; }

    public bool IsHide { get; init; }
}