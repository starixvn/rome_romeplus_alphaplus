using AgencyCash.API.Payments.Criteria;

namespace AgencyCash.API.Tests.Payments.Criteria;

[Trait("Category", "Payments")]
public class WriteLogTransferCriteriaTest
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly()
    {
        // Arrange
        var userId = 123;
        var fromCustId = 456;
        var toCustId = 789;
        var ip = "192.168.1.1";
        var doing = "Transfer funds";

        // Act
        var criteria = new WriteLogTransferCriteria(userId, fromCustId, toCustId, ip, doing);

        // Assert
        Assert.Equal(userId, criteria.UserId);
        Assert.Equal(fromCustId, criteria.FromCustId);
        Assert.Equal(toCustId, criteria.ToCustId);
        Assert.Equal(ip, criteria.IP);
        Assert.Equal(doing, criteria.Doing);
        Assert.Null(criteria.SiteDBGroupId);
    }

    [Fact]
    public void GetSettingKey_Should_Return_Default_Key_When_SiteDBGroupId_Not_Set()
    {
        // Arrange
        var criteria = new WriteLogTransferCriteria(1, 2, 3, "127.0.0.1", "Test transfer");

        // Act
        var key = criteria.GetSettingKey();

        // Assert
        Assert.Equal("DBLOG_WriteLog_CashService_UserLog_Transfer", key);
    }

    [Fact]
    public void GetSettingKey_Should_Return_SiteDB_Key_When_SiteDBGroupId_Is_Set()
    {
        // Arrange
        var criteria = new WriteLogTransferCriteria(1, 2, 3, "127.0.0.1", "Test transfer")
        {
            SiteDBGroupId = 5
        };

        // Act
        var key = criteria.GetSettingKey();

        // Assert
        Assert.Equal("SiteDB5_WriteLog_CashService_UserLog_Transfer", key);
    }

    [Fact]
    public void IsValid_Should_Return_True_When_All_Required_Fields_Are_Valid()
    {
        // Arrange
        var criteria = new WriteLogTransferCriteria(1, 2, 3, "127.0.0.1", "Test transfer");

        // Act & Assert
        Assert.True(criteria.IsValid());
    }

    [Theory]
    [InlineData(0, 2, 3, "127.0.0.1", "Test", false)]
    [InlineData(1, 0, 3, "127.0.0.1", "Test", false)]
    [InlineData(1, 2, 0, "127.0.0.1", "Test", false)]
    [InlineData(1, 2, 3, "", "Test", false)]
    [InlineData(1, 2, 3, "127.0.0.1", "", false)]
    [InlineData(1, 2, 3, null, "Test", false)]
    [InlineData(1, 2, 3, "127.0.0.1", null, false)]
    public void IsValid_Should_Return_Expected_Result_Based_On_Input(
        int userId, int fromCustId, int toCustId, string ip, string doing, bool expectedResult)
    {
        // Arrange
        var criteria = new WriteLogTransferCriteria(userId, fromCustId, toCustId, ip, doing);

        // Act
        var result = criteria.IsValid();

        // Assert
        Assert.Equal(expectedResult, result);
    }
}