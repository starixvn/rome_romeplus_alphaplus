using AlphaPlus.Settings.MemberInfo.Domain.Params;

namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class ResetCurrentWinAmountParamTests
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

        // Act
        var param = new ResetCurrentWinAmountParam(
            custId, productId, sportType, betType, subAccId, subAccName,
            userId, ip, settingStatus, username, language);

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
        var param = new ResetCurrentWinAmountParam(
            123, 456, 1, 2, 789, subAccName, 101, ip, 3, username, language);

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
        var param = new ResetCurrentWinAmountParam(
            123, 456, 1, 2, 789, subAccName, 101, ip, 3, username, language);

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
        var param = CreateResetCurrentWinAmountParam();

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
    }

    [Fact]
    public void Constructor_With_Zero_Values_Should_Work()
    {
        // Act
        var param = new ResetCurrentWinAmountParam(
            0, 0, 0, 0, 0, "sub", 0, "192.168.1.1", 0, "user", "en");

        // Assert
        Assert.Equal(0, param.CustId);
        Assert.Equal(0, param.ProductId);
        Assert.Equal(0, param.SportType);
        Assert.Equal(0, param.BetType);
        Assert.Equal(0, param.SubAccId);
        Assert.Equal(0, param.UserId);
        Assert.Equal(0, param.SettingStatus);
    }

    [Fact]
    public void Constructor_With_Negative_Values_Should_Work()
    {
        // Act
        var param = new ResetCurrentWinAmountParam(
            -1, -2, -3, -4, -5, "sub", -6, "192.168.1.1", -7, "user", "en");

        // Assert
        Assert.Equal(-1, param.CustId);
        Assert.Equal(-2, param.ProductId);
        Assert.Equal(-3, param.SportType);
        Assert.Equal(-4, param.BetType);
        Assert.Equal(-5, param.SubAccId);
        Assert.Equal(-6, param.UserId);
        Assert.Equal(-7, param.SettingStatus);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, "", 0, "", 0, "", "")]
    [InlineData(1, 2, 3, 4, 5, "sub1", 6, "192.168.1.1", 7, "user1", "en")]
    [InlineData(-1, -2, -3, -4, -5, "sub2", -6, "10.0.0.1", -7, "user2", "vi")]
    [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, "maxsub", int.MaxValue, "255.255.255.255", int.MaxValue, "maxuser", "zh")]
    public void Constructor_With_Various_Values_Should_Work(
        int custId, int productId, int sportType, int betType, int subAccId,
        string subAccName, int userId, string ip, int settingStatus,
        string username, string language)
    {
        // Act
        var param = new ResetCurrentWinAmountParam(
            custId, productId, sportType, betType, subAccId, subAccName,
            userId, ip, settingStatus, username, language);

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
    }

    [Fact]
    public void Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var param1 = new ResetCurrentWinAmountParam(
            1, 2, 3, 4, 5, "sub1", 6, "192.168.1.1", 7, "user1", "en");
        var param2 = new ResetCurrentWinAmountParam(
            10, 20, 30, 40, 50, "sub2", 60, "10.0.0.1", 70, "user2", "vi");

        // Assert
        Assert.Equal(1, param1.CustId);
        Assert.Equal("sub1", param1.SubAccName);
        Assert.Equal("user1", param1.Username);
        
        Assert.Equal(10, param2.CustId);
        Assert.Equal("sub2", param2.SubAccName);
        Assert.Equal("user2", param2.Username);
    }

    [Fact]
    public void Constructor_Should_Not_Modify_Input_Values()
    {
        // Arrange
        var originalCustId = 123;
        var originalProductId = 456;
        var originalSubAccName = "SubAccount";
        var custId = originalCustId;
        var productId = originalProductId;
        var subAccName = originalSubAccName;

        // Act
        var param = new ResetCurrentWinAmountParam(
            custId, productId, 1, 2, 789, subAccName, 101, "192.168.1.1", 3, "user", "en");

        // Assert
        Assert.Equal(originalCustId, custId);
        Assert.Equal(originalProductId, productId);
        Assert.Equal(originalSubAccName, subAccName);
        Assert.Equal(originalCustId, param.CustId);
        Assert.Equal(originalProductId, param.ProductId);
        Assert.Equal(originalSubAccName, param.SubAccName);
    }

    [Fact]
    public void Constructor_With_Long_String_Values_Should_Work()
    {
        // Arrange
        var longSubAccName = new string('a', 1000);
        var longUsername = new string('b', 1000);
        var longLanguage = new string('c', 1000);

        // Act
        var param = new ResetCurrentWinAmountParam(
            123, 456, 1, 2, 789, longSubAccName, 101, "192.168.1.1", 3, longUsername, longLanguage);

        // Assert
        Assert.Equal(longSubAccName, param.SubAccName);
        Assert.Equal(longUsername, param.Username);
        Assert.Equal(longLanguage, param.Language);
        Assert.Equal(1000, param.SubAccName.Length);
        Assert.Equal(1000, param.Username.Length);
        Assert.Equal(1000, param.Language.Length);
    }

    [Fact]
    public void Constructor_With_Special_Characters_Should_Work()
    {
        // Arrange
        var specialSubAccName = "sub@acc!#$%^&*()_+{}|:<>?[];',./";
        var specialUsername = "user@name!#$%^&*()_+{}|:<>?[];',./";
        var specialLanguage = "lang@uage!#$%^&*()_+{}|:<>?[];',./";

        // Act
        var param = new ResetCurrentWinAmountParam(
            123, 456, 1, 2, 789, specialSubAccName, 101, "192.168.1.1", 3, specialUsername, specialLanguage);

        // Assert
        Assert.Equal(specialSubAccName, param.SubAccName);
        Assert.Equal(specialUsername, param.Username);
        Assert.Equal(specialLanguage, param.Language);
    }

    private static ResetCurrentWinAmountParam CreateResetCurrentWinAmountParam()
    {
        return new ResetCurrentWinAmountParam(
            123, 456, 1, 2, 789, "SubAccount", 101, "192.168.1.1", 3, "testuser", "en");
    }
}
