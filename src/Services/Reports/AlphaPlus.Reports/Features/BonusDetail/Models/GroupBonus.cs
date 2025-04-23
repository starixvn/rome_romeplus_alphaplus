namespace AlphaPlus.Reports.Features.BonusDetail;

public class GroupBonus
{
    public GroupBonus(decimal totalAmount)
        => TotalAmount = totalAmount;

    public GroupBonus(string groupBy, decimal totalAmount, IList<BonusEntity> bonus)
    {
        GroupBy = groupBy;
        TotalAmount = totalAmount;
        Bonus = bonus;
        GroupBonusList = new List<GroupBonus>();
    }

    public GroupBonus(string groupBy, decimal totalAmount, IList<GroupBonus> groupBonusList, int colspan)
    {
        GroupBy = groupBy;
        TotalAmount = totalAmount;
        GroupBonusList = groupBonusList;
        ColSpan = colspan;
        Bonus = new List<BonusEntity>();
    }

    public string GroupBy { get; }

    public decimal TotalAmount { get; }

    public IList<BonusEntity> Bonus { get; }

    public IList<GroupBonus> GroupBonusList { get; }

    public int ColSpan { get; set; }
}