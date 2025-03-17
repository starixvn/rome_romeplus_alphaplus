using AlphaPlus.Reports.Features.BonusDetail;

namespace AlphaPlus.Reports.UnitTests.Features.BonusDetail;

[Trait("Category", "BonusDetail")]
public class GroupBonusTests
{
    [Fact]
    public void Constructor_ShouldInitializeTotalAmountCorrectly()
    {
        // Arrange
        var totalAmount = 100.50m;

        // Act
        var groupBonus = new GroupBonus(totalAmount);

        // Assert
        Assert.Equal(totalAmount, groupBonus.TotalAmount);
        Assert.Null(groupBonus.GroupBy);
        Assert.Null(groupBonus.Bonus);
        Assert.Null(groupBonus.GroupBonusList);
        Assert.Equal(0, groupBonus.ColSpan);
    }

    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly_WithBonusList()
    {
        // Arrange
        var groupBy = "Group1";
        var totalAmount = 200.75m;
        var bonusList = new List<BonusEntity>
        {
            new BonusEntity { Username = "user1", Amount = 50.25m },
            new BonusEntity { Username = "user2", Amount = 150.50m }
        };

        // Act
        var groupBonus = new GroupBonus(groupBy, totalAmount, bonusList);

        // Assert
        Assert.Equal(groupBy, groupBonus.GroupBy);
        Assert.Equal(totalAmount, groupBonus.TotalAmount);
        Assert.Equal(bonusList, groupBonus.Bonus);
        Assert.Empty(groupBonus.GroupBonusList);
        Assert.Equal(0, groupBonus.ColSpan);
    }

    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly_WithGroupBonusList()
    {
        // Arrange
        var groupBy = "Group2";
        var totalAmount = 300.00m;
        var groupBonusList = new List<GroupBonus>
        {
            new GroupBonus("SubGroup1", 100.00m, new List<BonusEntity>()),
            new GroupBonus("SubGroup2", 200.00m, new List<BonusEntity>())
        };
        var colspan = 2;

        // Act
        var groupBonus = new GroupBonus(groupBy, totalAmount, groupBonusList, colspan);

        // Assert
        Assert.Equal(groupBy, groupBonus.GroupBy);
        Assert.Equal(totalAmount, groupBonus.TotalAmount);
        Assert.Empty(groupBonus.Bonus);
        Assert.Equal(groupBonusList, groupBonus.GroupBonusList);
        Assert.Equal(colspan, groupBonus.ColSpan);
    }
}
