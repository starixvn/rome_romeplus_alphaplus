namespace AlphaPlus.Settings.MemberInfo.Domain.Models;

public class Product
{
    public int Id { get; init; }

    public string Name { get; set; }

    public bool IsDisable { get; init; }

    public DateTime? EndOfSupportDate { get; init; }

    public int OrderNo { get; init; }

    public int Type { get; init; }

    public int GroupId { get; init; }
}
