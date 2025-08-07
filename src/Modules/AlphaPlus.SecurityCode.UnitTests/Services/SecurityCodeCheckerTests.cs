namespace AlphaPlus.SecurityCode.UnitTests.Services;

public class SecurityCodeCheckerTests
{
    private readonly Mock<IOptions<SecurityCodeOptions>> _mockOptions;
    private readonly SecurityCodeOptions _options;
    private readonly Mock<ISecurityCodeSession> _mockSession;

    public SecurityCodeCheckerTests()
    {
        _mockOptions = new Mock<IOptions<SecurityCodeOptions>>();
        _options = new SecurityCodeOptions();
        _mockOptions.Setup(x => x.Value).Returns(_options);

        _mockSession = new Mock<ISecurityCodeSession>();
        SecurityCodeSessionManager.SecurityCodeSession = _mockSession.Object;
    }

    [Fact]
    public void IsEnableSecurityCode_WhenUserSiteInSites_ShouldReturnTrue()
    {
        // Arrange
        _options.Sites = new[] { "site1", "site2" };
        _mockSession.Setup(x => x.IsForcedChangeSecurityCode).Returns(false);
        _mockSession.Setup(x => x.UserSite).Returns("site1");
        _mockSession.Setup(x => x.SecurityCode).Returns("123456");

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsEnableSecurityCode();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEnableSecurityCode_WhenNoSecurityCode_ShouldReturnFalse()
    {
        // Arrange
        _mockSession.Setup(x => x.IsForcedChangeSecurityCode).Returns(false);
        _mockSession.Setup(x => x.SecurityCode).Returns("");

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsEnableSecurityCode();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsForcedChangeSecurityCode_WhenNotInternalAndForcedChange_ShouldReturnTrue()
    {
        // Arrange
        _mockSession.Setup(x => x.IsInternal).Returns(false);
        _mockSession.Setup(x => x.IsForcedChangeSecurityCode).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsForcedChangeSecurityCode();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsForcedChangeSecurityCode_WhenInternal_ShouldReturnFalse()
    {
        // Arrange
        _mockSession.Setup(x => x.IsInternal).Returns(true);
        _mockSession.Setup(x => x.IsForcedChangeSecurityCode).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsForcedChangeSecurityCode();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenAlreadyInputSecurityCode_ShouldReturnFalse()
    {
        // Arrange
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("/api/test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenInternal_ShouldReturnFalse()
    {
        // Arrange
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(false);
        _mockSession.Setup(x => x.IsInternal).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("/api/test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenNotEnabledOtpAndNotEnabledSecurityCode_ShouldReturnFalse()
    {
        // Arrange
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(false);
        _mockSession.Setup(x => x.IsInternal).Returns(false);
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("/api/test");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenEmptyPath_ShouldReturnFalse()
    {
        // Arrange
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(false);
        _mockSession.Setup(x => x.IsInternal).Returns(false);
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenPathMatchesSecurityPaths_ShouldReturnTrue()
    {
        // Arrange
        _options.Paths = new[] { "/api/secure", "/admin" };
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(false);
        _mockSession.Setup(x => x.IsInternal).Returns(false);
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("/api/secure");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenPathMatchesBetListsPaths_ShouldReturnTrue()
    {
        // Arrange
        _options.BetListsPaths = new[] { "/bets", "/history" };
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(false);
        _mockSession.Setup(x => x.IsInternal).Returns(false);
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(true);
        _mockSession.Setup(x => x.IsEnabledSecurityCodeInBetLists).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("/bets/list");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenPathMatchesStatementPaths_ShouldReturnTrue()
    {
        // Arrange
        _options.StatementPaths = new[] { "/statement", "/reports" };
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(false);
        _mockSession.Setup(x => x.IsInternal).Returns(false);
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(true);
        _mockSession.Setup(x => x.IsEnabledSecurityCodeInStatement).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("/statement/detail");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRequiredSecurityCode_WhenPathDoesNotMatchAnyPaths_ShouldReturnFalse()
    {
        // Arrange
        _options.Paths = new[] { "/api/secure" };
        _options.BetListsPaths = new[] { "/bets" };
        _options.StatementPaths = new[] { "/statement" };
        _mockSession.Setup(x => x.IsAlreadyInputSecurityCode).Returns(false);
        _mockSession.Setup(x => x.IsInternal).Returns(false);
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(true);
        _mockSession.Setup(x => x.IsEnabledSecurityCodeInBetLists).Returns(true);
        _mockSession.Setup(x => x.IsEnabledSecurityCodeInStatement).Returns(true);

        var checker = new SecurityCodeChecker(_mockOptions.Object);

        // Act
        var result = checker.IsRequiredSecurityCode("/api/public");

        // Assert
        Assert.False(result);
    }
}
