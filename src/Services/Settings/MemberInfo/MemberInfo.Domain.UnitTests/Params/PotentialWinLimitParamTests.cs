namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class PotentialWinLimitParamTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var roleId = (byte)1;
        var username = "testuser";
        var status = "Open";
        var problemAccount = (byte)0;
        var pageSize = 10;
        var pageIndex = 0;
        var agIP = "192.168.1.1";
        var betType = 1;
        var sportType = 2;
        var accountType = 3;
        var settingStatus = 4;

        // Act
        var param = new PotentialWinLimitParam(
            custId, roleId, username, status, problemAccount,
            pageSize, pageIndex, agIP, betType, sportType, accountType, settingStatus);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(roleId, param.RoleId);
        Assert.Equal(username, param.Username);
        Assert.Equal(status, param.Status);
        Assert.Equal(problemAccount, param.ProblemAccount);
        Assert.Equal(pageSize, param.PageSize);
        Assert.Equal(pageIndex, param.PageIndex);
        Assert.Equal(siteDbGroupId, param.SiteDbGroupId);
        Assert.Equal(betType, param.BetType);
        Assert.Equal(sportType, param.SportType);
        Assert.Equal(accountType, param.AccountType);
        Assert.Equal(settingStatus, param.SettingStatus);
    }

    [Fact]
    public void Constructor_With_Null_Status_Should_Set_Status_To_All()
    {
        // Arrange
        string status = null;

        // Act
        var param = CreatePotentialWinLimitParam(status: status);

        // Assert
        Assert.Equal("All", param.Status);
    }

    [Fact]
    public void Constructor_With_Empty_Status_Should_Set_Status_To_All()
    {
        // Arrange
        var status = string.Empty;

        // Act
        var param = CreatePotentialWinLimitParam(status: status);

        // Assert
        Assert.Equal("All", param.Status);
    }

    [Fact]
    public void Constructor_With_Valid_Status_Should_Keep_Status()
    {
        // Arrange
        var status = "Open";

        // Act
        var param = CreatePotentialWinLimitParam(status: status);

        // Assert
        Assert.Equal("Open", param.Status);
    }

    [Fact]
    public void Constructor_With_Default_Optional_Parameters_Should_Set_Zero_Values()
    {
        // Arrange & Act
        var param = new PotentialWinLimitParam(
            custId: 123,
            roleId: 1,
            username: "test",
            status: "Open",
            problemAccount: 0,
            pageSize: 10,
            pageIndex: 0,
            agIP: "192.168.1.1");

        // Assert
        Assert.Equal(0, param.BetType);
        Assert.Equal(0, param.SportType);
        Assert.Equal(0, param.AccountType);
        Assert.Equal(0, param.SettingStatus);
    }

    [Fact]
    public void Constructor_With_All_Optional_Parameters_Should_Set_Correct_Values()
    {
        // Arrange & Act
        var param = new PotentialWinLimitParam(
            custId: 123,
            roleId: 1,
            username: "test",
            status: "Open",
            problemAccount: 0,
            pageSize: 10,
            pageIndex: 0,
            betType: 5,
            sportType: 6,
            accountType: 7,
            settingStatus: 8);

        // Assert
        Assert.Equal(5, param.BetType);
        Assert.Equal(6, param.SportType);
        Assert.Equal(7, param.AccountType);
        Assert.Equal(8, param.SettingStatus);
    }

    [Fact]
    public void AGIP_Property_Should_Be_ReadOnly()
    {
        // Arrange
        var agIP = "192.168.1.1";

        // Act
        var param = CreatePotentialWinLimitParam(agIP: agIP);

        // Assert
        Assert.Equal(agIP, param.AGIP);
        
        // Verify property is read-only
        var agIPProperty = typeof(PotentialWinLimitParam).GetProperty(nameof(PotentialWinLimitParam.AGIP));
        Assert.True(agIPProperty.CanRead);
        Assert.False(agIPProperty.CanWrite);
    }

    [Fact]
    public void Settable_Properties_Should_Allow_Modification()
    {
        // Arrange
        var param = CreatePotentialWinLimitParam();

        // Act
        param.CustId = 999;
        param.RoleId = 5;
        param.BetType = 10;
        param.SportType = 11;
        param.Username = "newuser";
        param.Status = "Closed";
        param.ProblemAccount = 1;
        param.PageSize = 20;
        param.PageIndex = 2;
        param.AccountType = 15;
        param.SettingStatus = 16;

        // Assert
        Assert.Equal(999, param.CustId);
        Assert.Equal(5, param.RoleId);
        Assert.Equal(10, param.BetType);
        Assert.Equal(11, param.SportType);
        Assert.Equal("newuser", param.Username);
        Assert.Equal("Closed", param.Status);
        Assert.Equal(1, param.ProblemAccount);
        Assert.Equal(20, param.PageSize);
        Assert.Equal(2, param.PageIndex);
        Assert.Equal(15, param.AccountType);
        Assert.Equal(16, param.SettingStatus);
    }

    [Theory]
    [InlineData(null, "All")]
    [InlineData("", "All")]
    [InlineData("   ", "   ")]
    [InlineData("Open", "Open")]
    [InlineData("Closed", "Closed")]
    [InlineData("Suspended", "Suspended")]
    public void Constructor_Should_Handle_Various_Status_Values(string inputStatus, string expectedStatus)
    {
        // Act
        var param = CreatePotentialWinLimitParam(status: inputStatus);

        // Assert
        Assert.Equal(expectedStatus, param.Status);
    }

    [Fact]
    public void Constructor_With_Null_Username_Should_Work()
    {
        // Arrange
        string username = null;

        // Act
        var param = CreatePotentialWinLimitParam(username: username);

        // Assert
        Assert.Null(param.Username);
    }

    [Fact]
    public void Constructor_With_Null_AGIP_Should_Work()
    {
        // Arrange
        string agIP = null;

        // Act
        var param = CreatePotentialWinLimitParam(agIP: agIP);

        // Assert
        Assert.Null(param.AGIP);
    }

    [Theory]
    [InlineData(-1, 0, -5, 100)]
    [InlineData(0, 255, 0, 0)]
    [InlineData(int.MaxValue, byte.MaxValue, int.MaxValue, int.MaxValue)]
    public void Constructor_With_Edge_Case_Values_Should_Work(int custId, byte roleId, int pageIndex, int pageSize)
    {
        // Act
        var param = new PotentialWinLimitParam(
            custId, roleId, "test", "Open", 0, pageSize, pageIndex, "192.168.1.1");

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(roleId, param.RoleId);
        Assert.Equal(pageIndex, param.PageIndex);
        Assert.Equal(pageSize, param.PageSize);
    }

    private static PotentialWinLimitParam CreatePotentialWinLimitParam(
        int custId = 123,
        byte roleId = 1,
        string username = "testuser",
        string status = "Open",
        byte problemAccount = 0,
        int pageSize = 10,
        int pageIndex = 0,
        string agIP = "192.168.1.1")
    {
        return new PotentialWinLimitParam(
            custId, roleId, username, status, problemAccount,
            pageSize, pageIndex, agIP);
    }
}