namespace AlphaPlus.Identity.API.UnitTests.Features.Auth.SiteDb;

public class GetSiteDBConfigCriteriaTests
{
    [Fact]
    public void Constructor_ShouldSetCustId()
    {
        // Arrange
        const int expectedCustId = 123;

        // Act
        var criteria = new GetSiteDBConfigCriteria(expectedCustId);

        // Assert
        criteria.CustId.Should().Be(expectedCustId);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(100, true)]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    public void IsValid_ShouldReturnExpectedResult(int custId, bool expectedResult)
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria(custId);

        // Act
        var result = criteria.IsValid();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectKey()
    {
        // Arrange
        var criteria = new GetSiteDBConfigCriteria(1);

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        result.Should().Be("Acc_ServerDBConfigSelByCust");
    }

    [Fact]
    public void GetSiteDBConfigCriteria_ShouldInheritFromCriteriaBase()
    {
        // Arrange & Act
        var criteria = new GetSiteDBConfigCriteria(1);

        // Assert
        criteria.Should().BeAssignableTo<Fanex.Data.Repository.CriteriaBase>();
    }
}
