using AgencyCash.API.Payments.Criteria;

namespace AgencyCash.API.Tests.Payments.Criteria;

public class SyncCashAccumCreditCriteriaTest
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenInitialized()
    {
        // Arrange
        const string cashAccumCreditTbl = "TestTable";
        const short custDBGroupId = 2;

        // Act
        var criteria = new SyncCashAccumCreditCriteria(cashAccumCreditTbl, custDBGroupId);

        // Assert
        Assert.Equal(cashAccumCreditTbl, criteria.CashAccumCreditTbl);
    }

    [Fact]
    public void GetSettingKey_ShouldReturnCorrectSPName()
    {
        // Arrange
        const string cashAccumCreditTbl = "TestTable";
        const short custDBGroupId = 3;
        var criteria = new SyncCashAccumCreditCriteria(cashAccumCreditTbl, custDBGroupId);
        var expectedKey = "CustDB3_Age_CashAccumCredit_SyncData";

        // Act
        var actualKey = criteria.GetSettingKey();

        // Assert
        Assert.Equal(expectedKey, actualKey);
    }

    [Theory]
    [InlineData("TestTable", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(null, false)]
    public void IsValid_ShouldReturnExpectedResult(string cashAccumCreditTbl, bool expectedResult)
    {
        // Arrange
        var criteria = new SyncCashAccumCreditCriteria(cashAccumCreditTbl, 1);

        // Act
        var isValid = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, isValid);
    }

    [Fact]
    public void ErrorProperties_ShouldBeSettable()
    {
        // Arrange
        var criteria = new SyncCashAccumCreditCriteria("TestTable", 1);
        const int errCode = 500;
        const string errMsg = "Test error message";

        // Act
        criteria.ErrCode = errCode;
        criteria.ErrMsg = errMsg;

        // Assert
        Assert.Equal(errCode, criteria.ErrCode);
        Assert.Equal(errMsg, criteria.ErrMsg);
    }
}