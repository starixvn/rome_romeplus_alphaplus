namespace AlphaPlus.SecurityCode.UnitTests.Services;

public class SecurityCodePathBuilderTests
{
    private readonly Mock<IOptions<SecurityCodeOptions>> _mockOptions;
    private readonly SecurityCodeOptions _options;
    private readonly Mock<ISecurityCodeSession> _mockSession;

    public SecurityCodePathBuilderTests()
    {
        _mockOptions = new Mock<IOptions<SecurityCodeOptions>>();
        _options = new SecurityCodeOptions
        {
            ResetSecurityCodePath = "/reset",
            VerifyingSecurityCodePath = "/verify",
            VerifyingOtpPath = "/otp"
        };
        _mockOptions.Setup(x => x.Value).Returns(_options);

        _mockSession = new Mock<ISecurityCodeSession>();
        SecurityCodeSessionManager.SecurityCodeSession = _mockSession.Object;
    }

    [Fact]
    public void BuildResetSecurityCodePath_ShouldReturnCorrectPath()
    {
        // Arrange
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);

        // Act
        var result = builder.BuildResetSecurityCodePath();

        // Assert
        Assert.Contains("/reset", result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WhenOtpEnabled_ShouldUseOtpPath()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(true);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);

        // Act
        var result = builder.BuildVerifyingSecurityCodePath();

        // Assert
        Assert.Contains("/otp", result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WhenOtpDisabled_ShouldUseSecurityCodePath()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);

        // Act
        var result = builder.BuildVerifyingSecurityCodePath();

        // Assert
        Assert.Contains("/verify", result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WithRedirectUrl_ShouldIncludeRedirectUrl()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);
        var redirectUrl = "/api/test";

        // Act
        var result = builder.BuildVerifyingSecurityCodePath(redirectUrl);

        // Assert
        Assert.Contains("redirectUrl=", result);
        Assert.Contains(System.Web.HttpUtility.UrlEncode(redirectUrl), result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WithQueryStringItems_ShouldIncludeQueryString()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);
        var queryStringItems = new Dictionary<string, string>
        {
            { "param1", "value1" },
            { "param2", "value2" }
        };

        // Act
        var result = builder.BuildVerifyingSecurityCodePath("", queryStringItems);

        // Assert
        Assert.Contains("param1=value1", result);
        Assert.Contains("param2=value2", result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WithRedirectUrlAndQueryString_ShouldIncludeBoth()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);
        var redirectUrl = "/api/test";
        var queryStringItems = new Dictionary<string, string>
        {
            { "param1", "value1" }
        };

        // Act
        var result = builder.BuildVerifyingSecurityCodePath(redirectUrl, queryStringItems);

        // Assert
        Assert.Contains("param1=value1", result);
        Assert.Contains("redirectUrl=", result);
        Assert.Contains(System.Web.HttpUtility.UrlEncode(redirectUrl), result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WithEmptyQueryStringItems_ShouldNotIncludeQueryString()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);
        var queryStringItems = new Dictionary<string, string>();

        // Act
        var result = builder.BuildVerifyingSecurityCodePath("", queryStringItems);

        // Assert
        Assert.DoesNotContain("?", result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WithNullQueryStringItems_ShouldNotIncludeQueryString()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);

        // Act
        var result = builder.BuildVerifyingSecurityCodePath("", null);

        // Assert
        Assert.DoesNotContain("?", result);
    }

    [Fact]
    public void BuildVerifyingSecurityCodePath_WithSpecialCharacters_ShouldUrlEncodeCorrectly()
    {
        // Arrange
        _mockSession.Setup(x => x.IsEnabledOtp).Returns(false);
        var builder = new SecurityCodePathBuilder(_mockOptions.Object);
        var redirectUrl = "/api/test?param=value with spaces";
        var queryStringItems = new Dictionary<string, string>
        {
            { "key with spaces", "value with & symbols" }
        };

        // Act
        var result = builder.BuildVerifyingSecurityCodePath(redirectUrl, queryStringItems);

        // Assert
        Assert.Contains(System.Web.HttpUtility.UrlEncode("key with spaces"), result);
        Assert.Contains(System.Web.HttpUtility.UrlEncode("value with & symbols"), result);
        Assert.Contains(System.Web.HttpUtility.UrlEncode(redirectUrl), result);
    }
}
