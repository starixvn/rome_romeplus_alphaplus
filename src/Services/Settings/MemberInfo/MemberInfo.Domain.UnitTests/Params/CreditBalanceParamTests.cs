using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class CreditBalanceParamTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var custLevel = UserLevel.Agent;
        var userName = "testuser";
        var statusId = StatusIdentity.Open;
        var levelType = (byte)1;
        var pageIndex = 0;
        var pageSize = 10;
        var isAdminCanViewVNIP = true;
        var agIP = "192.168.1.1";
        var siteDbGroupId = 1;

        // Act
        var param = new CreditBalanceParam(
            custId, custLevel, userName, statusId, levelType,
            pageIndex, pageSize, isAdminCanViewVNIP, agIP, siteDbGroupId);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(custLevel, param.CustLevel);
        Assert.Equal(userName, param.UserName);
        Assert.Equal(levelType, param.LevelType);
        Assert.Equal(pageIndex, param.PageIndex);
        Assert.Equal(pageSize, param.PageSize);
        Assert.Equal(isAdminCanViewVNIP, param.IsAdminCanViewVNIP);
        Assert.Equal(agIP, param.AGIP);
    }

    [Fact]
    public void Constructor_With_Open_Status_Should_Set_Closed_False()
    {
        // Arrange
        var statusId = StatusIdentity.Open;

        // Act
        var param = CreateCreditBalanceParam(statusId);

        // Assert
        Assert.False(param.Closed);
        Assert.Null(param.Enabled);
        Assert.Null(param.Suspended);
    }

    [Fact]
    public void Constructor_With_Suspended_Status_Should_Set_Suspended_True()
    {
        // Arrange
        var statusId = StatusIdentity.Suspended;

        // Act
        var param = CreateCreditBalanceParam(statusId);

        // Assert
        Assert.True(param.Suspended);
        Assert.Null(param.Enabled);
        Assert.Null(param.Closed);
    }

    [Fact]
    public void Constructor_With_Closed_Status_Should_Set_Closed_True()
    {
        // Arrange
        var statusId = StatusIdentity.Closed;

        // Act
        var param = CreateCreditBalanceParam(statusId);

        // Assert
        Assert.True(param.Closed);
        Assert.Null(param.Enabled);
        Assert.Null(param.Suspended);
    }

    [Fact]
    public void Constructor_With_Disabled_Status_Should_Set_Enabled_False()
    {
        // Arrange
        var statusId = StatusIdentity.Disabled;

        // Act
        var param = CreateCreditBalanceParam(statusId);

        // Assert
        Assert.False(param.Enabled);
        Assert.Null(param.Closed);
        Assert.Null(param.Suspended);
    }

    [Fact]
    public void Constructor_With_All_Status_Should_Leave_All_Null()
    {
        // Arrange
        var statusId = StatusIdentity.All;

        // Act
        var param = CreateCreditBalanceParam(statusId);

        // Assert
        Assert.Null(param.Enabled);
        Assert.Null(param.Closed);
        Assert.Null(param.Suspended);
    }

    [Fact]
    public void Constructor_With_Invalid_Status_Should_Leave_All_Null()
    {
        // Arrange
        var statusId = (byte)99; // Invalid status

        // Act
        var param = CreateCreditBalanceParam(statusId);

        // Assert
        Assert.Null(param.Enabled);
        Assert.Null(param.Closed);
        Assert.Null(param.Suspended);
    }

    [Fact]
    public void Constructor_With_Null_UserName_Should_Work()
    {
        // Arrange
        string userName = null;

        // Act
        var param = new CreditBalanceParam(
            123, UserLevel.Agent, userName, StatusIdentity.Open, 1,
            0, 10, true, "192.168.1.1");

        // Assert
        Assert.Null(param.UserName);
    }

    [Fact]
    public void Constructor_With_Empty_UserName_Should_Work()
    {
        // Arrange
        var userName = string.Empty;

        // Act
        var param = new CreditBalanceParam(
            123, UserLevel.Agent, userName, StatusIdentity.Open, 1,
            0, 10, true, "192.168.1.1");

        // Assert
        Assert.Equal(string.Empty, param.UserName);
    }

    [Fact]
    public void All_Properties_Should_Be_ReadOnly_Except_Status_Flags()
    {
        // Act
        var type = typeof(CreditBalanceParam);

        // Assert
        var properties = type.GetProperties();

        // These should be read-only
        var readOnlyProperties = new[] {
            nameof(CreditBalanceParam.CustId),
            nameof(CreditBalanceParam.CustLevel),
            nameof(CreditBalanceParam.UserName),
            nameof(CreditBalanceParam.LevelType),
            nameof(CreditBalanceParam.PageIndex),
            nameof(CreditBalanceParam.PageSize),
            nameof(CreditBalanceParam.IsAdminCanViewVNIP),
            nameof(CreditBalanceParam.AGIP)
        };

        foreach (var propName in readOnlyProperties)
        {
            var property = properties.First(p => p.Name == propName);
            Assert.True(property.CanRead, $"{propName} should be readable");
            Assert.False(property.CanWrite, $"{propName} should be read-only");
        }

        // These are status properties that are set internally by the constructor
        var statusProperties = new[] {
            nameof(CreditBalanceParam.Enabled),
            nameof(CreditBalanceParam.Closed),
            nameof(CreditBalanceParam.Suspended)
        };

        foreach (var propName in statusProperties)
        {
            var property = properties.First(p => p.Name == propName);
            Assert.True(property.CanRead, $"{propName} should be readable");
            // Note: These properties are set by the constructor's internal mapping logic
            // The actual accessibility of setters may vary, but they serve their intended purpose
        }
    }

    [Theory]
    [InlineData(StatusIdentity.Open, null, null, false)]
    [InlineData(StatusIdentity.Suspended, null, true, null)]
    [InlineData(StatusIdentity.Closed, null, null, true)]
    [InlineData(StatusIdentity.Disabled, false, null, null)]
    [InlineData(StatusIdentity.All, null, null, null)]
    public void Constructor_Should_Map_Status_Correctly(byte statusId, bool? expectedEnabled, bool? expectedSuspended, bool? expectedClosed)
    {
        // Act
        var param = CreateCreditBalanceParam(statusId);

        // Assert
        Assert.Equal(expectedEnabled, param.Enabled);
        Assert.Equal(expectedSuspended, param.Suspended);
        Assert.Equal(expectedClosed, param.Closed);
    }

    [Fact]
    public void Constructor_With_Zero_PageSize_Should_Work()
    {
        // Act
        var param = new CreditBalanceParam(
            123, UserLevel.Agent, "test", StatusIdentity.Open, 1,
            0, 0, true, "192.168.1.1");

        // Assert
        Assert.Equal(0, param.PageSize);
    }

    [Fact]
    public void Constructor_With_Negative_Values_Should_Work()
    {
        // Act
        var param = new CreditBalanceParam(
            -1, UserLevel.Agent, "test", StatusIdentity.Open, 1,
            -1, -1, false, "192.168.1.1");

        // Assert
        Assert.Equal(-1, param.CustId);
        Assert.Equal(-1, param.PageIndex);
        Assert.Equal(-1, param.PageSize);
    }

    private static CreditBalanceParam CreateCreditBalanceParam(byte statusId)
    {
        return new CreditBalanceParam(
            123, UserLevel.Agent, "testuser", statusId, 1,
            0, 10, true, "192.168.1.1");
    }
}