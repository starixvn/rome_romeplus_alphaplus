using AlphaPlus.BetLists.Shared.Criteria;

namespace AlphaPlus.BetLists.UnitTests.Shared.Criteria;

[Trait("Category", "BetLists")]
public class NAPUserPermissionCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitialize_CustId()
    {
        // Arrange
        const int expectedCustId = 123;

        // Act
        var criteria = new NAPUserPermissionCriteria(expectedCustId);

        // Assert
        Assert.Equal(expectedCustId, criteria.CustId);
    }

    [Fact]
    public void GetSettingKey_ShouldReturn_CorrectKey()
    {
        // Arrange
        var criteria = new NAPUserPermissionCriteria(1);
        const string expectedKey = "Acc_Con_FileNameSelByCustID";

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldReturn_ExpectedResult(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new NAPUserPermissionCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
