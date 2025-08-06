using FluentAssertions;
using Main.Site.Features.Account;

namespace AlphaPlus.Main.Site.UnitTests.Features.Account;

[Trait("Category", "Account")]
public class SessionMismatchLogModelTests
{
    [Fact]
    public void SessionMismatchLogModel_ShouldHaveCorrectDefaultValues()
    {
        // Arrange & Act
        var model = new SessionMismatchLogModel();

        // Assert
        model.Event.Should().Be(string.Empty);
        model.OriginalSessionId.Should().Be(string.Empty);
        model.NewSessionId.Should().Be(string.Empty);
        model.Timestamp.Should().Be(string.Empty);
        model.Url.Should().Be(string.Empty);
        model.Referrer.Should().Be(string.Empty);
        model.UserAgent.Should().Be(string.Empty);
        model.RetryCount.Should().Be(0);
        model.MaxRetries.Should().Be(0);
        model.UserId.Should().Be(0);
        model.Username.Should().Be(string.Empty);
        model.SiteLabel.Should().Be(string.Empty);
        model.IpAddress.Should().Be(string.Empty);
    }

    [Fact]
    public void SessionMismatchLogModel_ShouldAllowSettingAllProperties()
    {
        // Arrange
        var model = new SessionMismatchLogModel();

        // Act
        model.Event = "test_event";
        model.OriginalSessionId = "original_session";
        model.NewSessionId = "new_session";
        model.Timestamp = "2024-01-15 10:30:00";
        model.Url = "https://example.com/test";
        model.Referrer = "https://example.com";
        model.UserAgent = "Test Browser";
        model.RetryCount = 3;
        model.MaxRetries = 5;
        model.UserId = 123;
        model.Username = "testuser";
        model.SiteLabel = "TestSite";
        model.IpAddress = "192.168.1.1";

        // Assert
        model.Event.Should().Be("test_event");
        model.OriginalSessionId.Should().Be("original_session");
        model.NewSessionId.Should().Be("new_session");
        model.Timestamp.Should().Be("2024-01-15 10:30:00");
        model.Url.Should().Be("https://example.com/test");
        model.Referrer.Should().Be("https://example.com");
        model.UserAgent.Should().Be("Test Browser");
        model.RetryCount.Should().Be(3);
        model.MaxRetries.Should().Be(5);
        model.UserId.Should().Be(123);
        model.Username.Should().Be("testuser");
        model.SiteLabel.Should().Be("TestSite");
        model.IpAddress.Should().Be("192.168.1.1");
    }

    [Fact]
    public void SessionMismatchLogModel_ShouldHandleNullValues()
    {
        // Arrange
        var model = new SessionMismatchLogModel();

        // Act
        model.Event = null!;
        model.OriginalSessionId = null!;
        model.NewSessionId = null!;
        model.Timestamp = null!;
        model.Url = null!;
        model.Referrer = null!;
        model.UserAgent = null!;
        model.Username = null!;
        model.SiteLabel = null!;
        model.IpAddress = null!;

        // Assert
        model.Event.Should().BeNull();
        model.OriginalSessionId.Should().BeNull();
        model.NewSessionId.Should().BeNull();
        model.Timestamp.Should().BeNull();
        model.Url.Should().BeNull();
        model.Referrer.Should().BeNull();
        model.UserAgent.Should().BeNull();
        model.Username.Should().BeNull();
        model.SiteLabel.Should().BeNull();
        model.IpAddress.Should().BeNull();
    }

    [Fact]
    public void SessionMismatchLogModel_ShouldHandleEmptyStrings()
    {
        // Arrange
        var model = new SessionMismatchLogModel();

        // Act
        model.Event = "";
        model.OriginalSessionId = "";
        model.NewSessionId = "";
        model.Timestamp = "";
        model.Url = "";
        model.Referrer = "";
        model.UserAgent = "";
        model.Username = "";
        model.SiteLabel = "";
        model.IpAddress = "";

        // Assert
        model.Event.Should().Be("");
        model.OriginalSessionId.Should().Be("");
        model.NewSessionId.Should().Be("");
        model.Timestamp.Should().Be("");
        model.Url.Should().Be("");
        model.Referrer.Should().Be("");
        model.UserAgent.Should().Be("");
        model.Username.Should().Be("");
        model.SiteLabel.Should().Be("");
        model.IpAddress.Should().Be("");
    }

    [Fact]
    public void SessionMismatchLogModel_ShouldHandleNegativeValues()
    {
        // Arrange
        var model = new SessionMismatchLogModel();

        // Act
        model.RetryCount = -1;
        model.MaxRetries = -5;
        model.UserId = -123;

        // Assert
        model.RetryCount.Should().Be(-1);
        model.MaxRetries.Should().Be(-5);
        model.UserId.Should().Be(-123);
    }

    [Fact]
    public void SessionMismatchLogModel_ShouldHandleLargeValues()
    {
        // Arrange
        var model = new SessionMismatchLogModel();

        // Act
        model.RetryCount = int.MaxValue;
        model.MaxRetries = int.MaxValue;
        model.UserId = int.MaxValue;

        // Assert
        model.RetryCount.Should().Be(int.MaxValue);
        model.MaxRetries.Should().Be(int.MaxValue);
        model.UserId.Should().Be(int.MaxValue);
    }

    [Fact]
    public void SessionMismatchLogModel_ShouldHandleSpecialCharactersInStrings()
    {
        // Arrange
        var model = new SessionMismatchLogModel();

        // Act
        model.Event = "test_event_with_special_chars_!@#$%^&*()";
        model.OriginalSessionId = "original_session_with_quotes\"'";
        model.NewSessionId = "new_session_with_backslashes\\";
        model.Timestamp = "2024-01-15 10:30:00";
        model.Url = "https://example.com/test?param=value&another=value";
        model.Referrer = "https://example.com/path/to/page";
        model.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        model.Username = "user@domain.com";
        model.SiteLabel = "Test Site Label";
        model.IpAddress = "192.168.1.100";

        // Assert
        model.Event.Should().Be("test_event_with_special_chars_!@#$%^&*()");
        model.OriginalSessionId.Should().Be("original_session_with_quotes\"'");
        model.NewSessionId.Should().Be("new_session_with_backslashes\\");
        model.Timestamp.Should().Be("2024-01-15 10:30:00");
        model.Url.Should().Be("https://example.com/test?param=value&another=value");
        model.Referrer.Should().Be("https://example.com/path/to/page");
        model.UserAgent.Should().Be("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        model.Username.Should().Be("user@domain.com");
        model.SiteLabel.Should().Be("Test Site Label");
        model.IpAddress.Should().Be("192.168.1.100");
    }
} 