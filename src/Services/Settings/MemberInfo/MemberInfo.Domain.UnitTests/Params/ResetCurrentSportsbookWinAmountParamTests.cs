using AlphaPlus.Settings.MemberInfo.Domain.Params;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class ResetCurrentSportsbookWinAmountParamTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var custId = 123;
        var productId = 456;
        var sportType = 1;
        var betType = 2;
        var subAccId = 789;
        var subAccName = "SubAccount";
        var userId = 101;
        var ip = "192.168.1.1";
        var settingStatus = 3;
        var username = "testuser";
        var language = "en";
        var siteDB = (short)10;

        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            custId, productId, sportType, betType, subAccId, subAccName,
            userId, ip, settingStatus, username, language, siteDB);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(sportType, param.SportType);
        Assert.Equal(betType, param.BetType);
        Assert.Equal(subAccId, param.SubAccId);
        Assert.Equal(subAccName, param.SubAccName);
        Assert.Equal(userId, param.UserId);
        Assert.Equal(ip, param.IP);
        Assert.Equal(settingStatus, param.SettingStatus);
        Assert.Equal(username, param.Username);
        Assert.Equal(language, param.Language);
        Assert.Equal(siteDB, param.SiteDB);
    }

    [Fact]
    public void Constructor_With_Null_String_Values_Should_Work()
    {
        // Arrange
        string subAccName = null;
        string ip = null;
        string username = null;
        string language = null;

        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            123, 456, 1, 2, 789, subAccName, 101, ip, 3, username, language, 10);

        // Assert
        Assert.Null(param.SubAccName);
        Assert.Null(param.IP);
        Assert.Null(param.Username);
        Assert.Null(param.Language);
    }

    [Fact]
    public void Constructor_With_Empty_String_Values_Should_Work()
    {
        // Arrange
        var subAccName = string.Empty;
        var ip = string.Empty;
        var username = string.Empty;
        var language = string.Empty;

        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            123, 456, 1, 2, 789, subAccName, 101, ip, 3, username, language, 10);

        // Assert
        Assert.Equal(string.Empty, param.SubAccName);
        Assert.Equal(string.Empty, param.IP);
        Assert.Equal(string.Empty, param.Username);
        Assert.Equal(string.Empty, param.Language);
    }

    [Fact]
    public void All_Properties_Should_Be_Settable()
    {
        // Arrange
        var param = CreateResetCurrentSportsbookWinAmountParam();

        // Act
        param.CustId = 999;
        param.ProductId = 888;
        param.SportType = 777;
        param.BetType = 666;
        param.SubAccId = 555;
        param.SubAccName = "NewSubAccount";
        param.UserId = 444;
        param.IP = "10.0.0.1";
        param.SettingStatus = 333;
        param.Username = "newuser";
        param.Language = "vi";
        param.SiteDB = 222;

        // Assert
        Assert.Equal(999, param.CustId);
        Assert.Equal(888, param.ProductId);
        Assert.Equal(777, param.SportType);
        Assert.Equal(666, param.BetType);
        Assert.Equal(555, param.SubAccId);
        Assert.Equal("NewSubAccount", param.SubAccName);
        Assert.Equal(444, param.UserId);
        Assert.Equal("10.0.0.1", param.IP);
        Assert.Equal(333, param.SettingStatus);
        Assert.Equal("newuser", param.Username);
        Assert.Equal("vi", param.Language);
        Assert.Equal(222, param.SiteDB);
    }

    [Fact]
    public void Constructor_With_Zero_Values_Should_Work()
    {
        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            0, 0, 0, 0, 0, "sub", 0, "192.168.1.1", 0, "user", "en", 0);

        // Assert
        Assert.Equal(0, param.CustId);
        Assert.Equal(0, param.ProductId);
        Assert.Equal(0, param.SportType);
        Assert.Equal(0, param.BetType);
        Assert.Equal(0, param.SubAccId);
        Assert.Equal(0, param.UserId);
        Assert.Equal(0, param.SettingStatus);
        Assert.Equal(0, param.SiteDB);
    }

    [Fact]
    public void Constructor_With_Negative_Values_Should_Work()
    {
        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            -1, -2, -3, -4, -5, "sub", -6, "192.168.1.1", -7, "user", "en", -8);

        // Assert
        Assert.Equal(-1, param.CustId);
        Assert.Equal(-2, param.ProductId);
        Assert.Equal(-3, param.SportType);
        Assert.Equal(-4, param.BetType);
        Assert.Equal(-5, param.SubAccId);
        Assert.Equal(-6, param.UserId);
        Assert.Equal(-7, param.SettingStatus);
        Assert.Equal(-8, param.SiteDB);
    }

    [Fact]
    public void Constructor_With_Max_Short_Value_Should_Work()
    {
        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            123, 456, 1, 2, 789, "sub", 101, "192.168.1.1", 3, "user", "en", short.MaxValue);

        // Assert
        Assert.Equal(short.MaxValue, param.SiteDB);
    }

    [Fact]
    public void Constructor_With_Min_Short_Value_Should_Work()
    {
        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            123, 456, 1, 2, 789, "sub", 101, "192.168.1.1", 3, "user", "en", short.MinValue);

        // Assert
        Assert.Equal(short.MinValue, param.SiteDB);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, "", 0, "", 0, "", "", 0)]
    [InlineData(1, 2, 3, 4, 5, "sub1", 6, "192.168.1.1", 7, "user1", "en", 8)]
    [InlineData(-1, -2, -3, -4, -5, "sub2", -6, "10.0.0.1", -7, "user2", "vi", -8)]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, "maxsub", int.MaxValue, "255.255.255.255", int.MaxValue, "maxuser", "zh", short.MaxValue)]
    public void Constructor_With_Various_Values_Should_Work(
        int custId, int productId, int sportType, int betType, int subAccId,
        string subAccName, int userId, string ip, int settingStatus,
        string username, string language, short siteDB)
    {
        // Act
        var param = new ResetCurrentSportsbookWinAmountParam(
            custId, productId, sportType, betType, subAccId, subAccName,
            userId, ip, settingStatus, username, language, siteDB);

        // Assert
        Assert.Equal(custId, param.CustId);
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(sportType, param.SportType);
        Assert.Equal(betType, param.BetType);
        Assert.Equal(subAccId, param.SubAccId);
        Assert.Equal(subAccName, param.SubAccName);
        Assert.Equal(userId, param.UserId);
        Assert.Equal(ip, param.IP);
        Assert.Equal(settingStatus, param.SettingStatus);
        Assert.Equal(username, param.Username);
        Assert.Equal(language, param.Language);
        Assert.Equal(siteDB, param.SiteDB);
    }

    [Fact]
    public void Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var param1 = new ResetCurrentSportsbookWinAmountParam(
            1, 2, 3, 4, 5, "sub1", 6, "192.168.1.1", 7, "user1", "en", 8);
        var param2 = new ResetCurrentSportsbookWinAmountParam(
            10, 20, 30, 40, 50, "sub2", 60, "10.0.0.1", 70, "user2", "vi", 80);

        // Assert
        Assert.Equal(1, param1.CustId);
        Assert.Equal("sub1", param1.SubAccName);
        Assert.Equal("user1", param1.Username);
        
        Assert.Equal(10, param2.CustId);
        Assert.Equal("sub2", param2.SubAccName);
        Assert.Equal("user2", param2.Username);
    }

    private static ResetCurrentSportsbookWinAmountParam CreateResetCurrentSportsbookWinAmountParam()
    {
        return new ResetCurrentSportsbookWinAmountParam(
            123, 456, 1, 2, 789, "SubAccount", 101, "192.168.1.1", 3, "testuser", "en", 10);
    }
}
