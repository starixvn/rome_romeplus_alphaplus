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
