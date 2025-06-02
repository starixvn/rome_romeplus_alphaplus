using AlphaPlus.Reports.Features.BonusDetail;

namespace AlphaPlus.Reports.UnitTests.Features.BonusDetail;

[Trait("Category", "BonusDetail")]
public class GetSuperBonusCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var username = "testuser";
        var fromDate = new DateTime(2025, 1, 1);
        var toDate = new DateTime(2025, 1, 31);
        byte? statusId = 1;
        bool? isComplained = true;
        var pageIndex = 0;
        var pageSize = 10;
        var superId = 123;

        // Act
        var criteria = new GetSuperBonusCriteria(username, fromDate, toDate, statusId, isComplained, pageIndex, pageSize, superId);

        // Assert
        Assert.Equal(username, criteria.Username);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(statusId, criteria.StatusId);
        Assert.Equal(isComplained, criteria.IsComplained);
        Assert.Equal(pageIndex, criteria.PageIndex);
        Assert.Equal(pageSize, criteria.PageSize);
        Assert.Equal(superId, criteria.SuperId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetSuperBonusCriteria("testuser", DateTime.Now, DateTime.Now, 1, true, 0, 10, 123);

        // Act
        var settingKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal("Age_Rpt_BonusDetail_Super", settingKey);
    }

    [Fact]
    public void IsValid_ShouldReturnTrue_WhenCriteriaIsValid()
    {
        // Arrange
        var criteria = new GetSuperBonusCriteria("testuser", new DateTime(2025, 1, 1), new DateTime(2025, 1, 31), 1, true, 0, 10, 123);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValid_ShouldReturnFalse_WhenSuperIdIsZero()
    {
        // Arrange
        var criteria = new GetSuperBonusCriteria("testuser", new DateTime(2025, 1, 1), new DateTime(2025, 1, 31), 1, true, 0, 10, 0);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.False(isValid);
    }
}
