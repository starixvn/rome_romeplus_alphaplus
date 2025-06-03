using AlphaPlus.Reports.Features.WinLossByProduct;

namespace AlphaPlus.Reports.UnitTests.Features.WinLossByProduct;

[Trait("Category", "WinLossByProduct")]
public class WinLossByProductCriteriaTest
{
    [Fact]
    public void Constructor_ShouldInitializeAllProperties()
    {
        // Arrange
        int custId = 123;
        DateTime fromDate = new DateTime(2023, 1, 1);
        DateTime toDate = new DateTime(2023, 1, 31);
        DateTime balanceUpDay = new DateTime(2023, 1, 30);
        DateTime moveBetsDay = new DateTime(2023, 1, 29);
        string productIds = "1,2,3";
        byte custLevelId = 1;
        string productDetails = "<products></products>";

        // Act
        var criteria = new WinLossByProductCriteria(
            custId,
            fromDate,
            toDate,
            balanceUpDay,
            moveBetsDay,
            productIds,
            custLevelId,
            productDetails);

        // Assert
        Assert.Equal(custId, criteria.CustId);
        Assert.Equal(fromDate, criteria.FromDate);
        Assert.Equal(toDate, criteria.ToDate);
        Assert.Equal(balanceUpDay, criteria.BalanceUpDay);
        Assert.Equal(moveBetsDay, criteria.MoveBetsDay);
        Assert.Equal(productIds, criteria.ProductIds);
        Assert.Equal(productDetails, criteria.ProductDetails);
    }

    [Theory]
    [InlineData(RoleIdConstants.Agent, "Age_Rpt_WinLossByProduct_Member")]
    public void GetSettingKey_ShouldReturnMemberKey_WhenCustLevelIdIsAgentLevel(byte custLevelId, string expectedKey)
    {
        // Arrange
        var criteria = new WinLossByProductCriteria(
            custId: 123,
            fromDate: DateTime.Now,
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now,
            moveBetsDay: DateTime.Now,
            productIds: "1,2,3",
            custLevelId: custLevelId,
            productDetails: "details");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(RoleIdConstants.Master, "Age_Rpt_WinLossByProduct_Agent")]
    public void GetSettingKey_ShouldReturnAgentKey_WhenCustLevelIdIsMasterLevel(byte custLevelId, string expectedKey)
    {
        // Arrange
        var criteria = new WinLossByProductCriteria(
            custId: 123,
            fromDate: DateTime.Now,
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now,
            moveBetsDay: DateTime.Now,
            productIds: "1,2,3",
            custLevelId: custLevelId,
            productDetails: "details");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }

    [Theory]
    [InlineData(RoleIdConstants.Super, "Age_Rpt_WinLossByProduct_Master")]
    [InlineData(9, "Age_Rpt_WinLossByProduct_Master")]
    public void GetSettingKey_ShouldReturnMasterKey_WhenCustLevelIdIsNotAgentOrMasterLevel(byte custLevelId, string expectedKey)
    {
        // Arrange
        var criteria = new WinLossByProductCriteria(
            custId: 123,
            fromDate: DateTime.Now,
            toDate: DateTime.Now,
            balanceUpDay: DateTime.Now,
            moveBetsDay: DateTime.Now,
            productIds: "1,2,3",
            custLevelId: custLevelId,
            productDetails: "details");

        // Act
        var result = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, result);
    }
}