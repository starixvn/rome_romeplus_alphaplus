namespace AlphaPlus.Settings.MemberInfo.Domain.UnitTests.Params;

public class WinLossLimitParamTests
{
    private const int SabaclubProductId = 42;

    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var productId = 123;
        var custId = 456;
        var betType = 1;
        var sportType = 2;
        var username = "testuser";
        var playerId = "player123";
        var masterId = 789;
        var agentId = 101;
        var pageSize = 10;
        var pageIndex = 0;
        var isDirectMember = true;
        var accountType = 3;
        var siteDbGroupId = 1;

        // Act
        var param = new WinLossLimitParam(
            productId, custId, betType, sportType, username, playerId,
            masterId, agentId, pageSize, pageIndex, isDirectMember, accountType, siteDbGroupId);

        // Assert
        Assert.Equal(productId, param.ProductId);
        Assert.Equal(custId, param.CustId);
        Assert.Equal(betType, param.BetType);
        Assert.Equal(sportType, param.SportType);
        Assert.Equal(username, param.Username);
        Assert.Equal(playerId, param.PlayerId);
        Assert.Equal(masterId, param.MasterId);
        Assert.Equal(siteDbGroupId, param.SiteDbGroupId);
        Assert.Equal(agentId, param.AgentId);
        Assert.Equal(pageSize, param.PageSize);
        Assert.Equal(pageIndex, param.PageIndex);
        Assert.Equal(isDirectMember, param.IsDirectMember);
        Assert.Equal(accountType, param.AccountType);
    }

    [Fact]
    public void IsSabaclub_Should_Return_True_When_ProductId_Is_42()
    {
        // Arrange & Act
        var param = CreateWinLossLimitParam(productId: SabaclubProductId);

        // Assert
        Assert.True(param.IsSabaclub);
    }

    [Fact]
    public void IsSabaclub_Should_Return_False_When_ProductId_Is_Not_42()
    {
        // Arrange & Act
        var param = CreateWinLossLimitParam(productId: 1);

        // Assert
        Assert.False(param.IsSabaclub);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(41, false)]
    [InlineData(42, true)]
    [InlineData(43, false)]
    [InlineData(100, false)]
    public void IsSabaclub_Should_Return_Correct_Value_For_Various_ProductIds(int productId, bool expected)
    {
        // Act
        var param = CreateWinLossLimitParam(productId: productId);

        // Assert
        Assert.Equal(expected, param.IsSabaclub);
    }

    [Fact]
    public void Constructor_With_Null_Values_Should_Work()
    {
        // Arrange
        string username = null;
        string playerId = null;

        // Act
        var param = new WinLossLimitParam(
            1, 2, 3, 4, username, playerId, 5, 6, 10, 0, true, 7, 1);

        // Assert
        Assert.Null(param.Username);
        Assert.Null(param.PlayerId);
    }

    [Fact]
    public void Constructor_With_Empty_Strings_Should_Work()
    {
        // Arrange
        var username = string.Empty;
        var playerId = string.Empty;

        // Act
        var param = new WinLossLimitParam(
            1, 2, 3, 4, username, playerId, 5, 6, 10, 0, true, 7, 1);

        // Assert
        Assert.Equal(string.Empty, param.Username);
        Assert.Equal(string.Empty, param.PlayerId);
    }

    [Fact]
    public void ReadOnly_Properties_Should_Not_Be_Settable()
    {
        // Arrange
        var type = typeof(WinLossLimitParam);
        var readOnlyProperties = new[] {
            nameof(WinLossLimitParam.IsDirectMember),
            nameof(WinLossLimitParam.IsSabaclub)
        };

        // Act & Assert
        foreach (var propName in readOnlyProperties)
        {
            var property = type.GetProperty(propName);
            Assert.True(property.CanRead, $"{propName} should be readable");
            Assert.False(property.CanWrite, $"{propName} should be read-only");
        }
    }

    [Fact]
    public void Settable_Properties_Should_Allow_Modification()
    {
        // Arrange
        var param = CreateWinLossLimitParam();

        // Act
        param.ProductId = 999;
        param.CustId = 888;
        param.BetType = 777;
        param.SportType = 666;
        param.Username = "newuser";
        param.PlayerId = "newplayer";
        param.MasterId = 555;
        param.AgentId = 444;
        param.PageSize = 20;
        param.PageIndex = 2;
        param.AccountType = 333;

        // Assert
        Assert.Equal(999, param.ProductId);
        Assert.Equal(888, param.CustId);
        Assert.Equal(777, param.BetType);
        Assert.Equal(666, param.SportType);
        Assert.Equal("newuser", param.Username);
        Assert.Equal("newplayer", param.PlayerId);
        Assert.Equal(555, param.MasterId);
        Assert.Equal(444, param.AgentId);
        Assert.Equal(20, param.PageSize);
        Assert.Equal(2, param.PageIndex);
        Assert.Equal(333, param.AccountType);
    }

    [Fact]
    public void IsSabaclub_Should_Update_When_ProductId_Changes()
    {
        // Arrange
        var param = CreateWinLossLimitParam(productId: 1);
        Assert.False(param.IsSabaclub);

        // Act
        param.ProductId = SabaclubProductId;

        // Assert
        Assert.True(param.IsSabaclub);
    }

    [Fact]
    public void Constructor_With_Zero_Values_Should_Work()
    {
        // Act
        var param = new WinLossLimitParam(
            0, 0, 0, 0, "test", "player", 0, 0, 0, 0, false, 0, 0);

        // Assert
        Assert.Equal(0, param.ProductId);
        Assert.Equal(0, param.CustId);
        Assert.Equal(0, param.BetType);
        Assert.Equal(0, param.SportType);
        Assert.Equal(0, param.MasterId);
        Assert.Equal(0, param.AgentId);
        Assert.Equal(0, param.PageSize);
        Assert.Equal(0, param.PageIndex);
        Assert.Equal(0, param.AccountType);
        Assert.False(param.IsDirectMember);
        Assert.Equal(0, param.SiteDbGroupId);
    }

    [Fact]
    public void Constructor_With_Negative_Values_Should_Work()
    {
        // Act
        var param = new WinLossLimitParam(
            -1, -2, -3, -4, "test", "player", -5, -6, -7, -8, false, -9, -1);

        // Assert
        Assert.Equal(-1, param.ProductId);
        Assert.Equal(-2, param.CustId);
        Assert.Equal(-3, param.BetType);
        Assert.Equal(-4, param.SportType);
        Assert.Equal(-5, param.MasterId);
        Assert.Equal(-6, param.AgentId);
        Assert.Equal(-7, param.PageSize);
        Assert.Equal(-8, param.PageIndex);
        Assert.Equal(-9, param.AccountType);
        Assert.Equal(-1, param.SiteDbGroupId);
    }

    [Fact]
    public void Constructor_With_Max_Values_Should_Work()
    {
        // Act
        var param = new WinLossLimitParam(
            int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue,
            "test", "player", int.MaxValue, int.MaxValue, int.MaxValue,
            int.MaxValue, true, int.MaxValue, int.MaxValue);

        // Assert
        Assert.Equal(int.MaxValue, param.ProductId);
        Assert.Equal(int.MaxValue, param.CustId);
        Assert.Equal(int.MaxValue, param.BetType);
        Assert.Equal(int.MaxValue, param.SportType);
        Assert.Equal(int.MaxValue, param.MasterId);
        Assert.Equal(int.MaxValue, param.AgentId);
        Assert.Equal(int.MaxValue, param.PageSize);
        Assert.Equal(int.MaxValue, param.PageIndex);
        Assert.Equal(int.MaxValue, param.AccountType);
        Assert.Equal(int.MaxValue, param.SiteDbGroupId);
        Assert.True(param.IsDirectMember);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_Should_Preserve_IsDirectMember_Value(bool isDirectMember)
    {
        // Act
        var param = CreateWinLossLimitParam(isDirectMember: isDirectMember);

        // Assert
        Assert.Equal(isDirectMember, param.IsDirectMember);
    }

    [Fact]
    public void Multiple_Instances_Should_Be_Independent()
    {
        // Arrange & Act
        var param1 = CreateWinLossLimitParam(productId: SabaclubProductId, custId: 1);
        var param2 = CreateWinLossLimitParam(productId: 1, custId: 2);

        // Assert
        Assert.True(param1.IsSabaclub);
        Assert.False(param2.IsSabaclub);
        Assert.Equal(1, param1.CustId);
        Assert.Equal(2, param2.CustId);
    }

    private static WinLossLimitParam CreateWinLossLimitParam(
        int productId = 1,
        int custId = 123,
        int betType = 1,
        int sportType = 2,
        string username = "testuser",
        string playerId = "player123",
        int masterId = 456,
        int agentId = 789,
        int pageSize = 10,
        int pageIndex = 0,
        bool isDirectMember = true,
        int accountType = 1,
        int siteDbGroupId = 1)
    {
        return new WinLossLimitParam(
            productId, custId, betType, sportType, username, playerId,
            masterId, agentId, pageSize, pageIndex, isDirectMember, accountType, siteDbGroupId);
    }
}