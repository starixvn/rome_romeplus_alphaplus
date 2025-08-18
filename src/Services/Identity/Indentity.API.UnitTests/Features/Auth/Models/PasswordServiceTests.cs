namespace AlphaPlus.Identity.API.UnitTests.Features.Auth.Models;

public class PasswordServiceTests
{
    [Fact]
    public void PasswordService_Properties_ShouldBeSettable()
    {
        // Arrange
        var passwordService = new PasswordService
        {
            IsEnabled = true,
            Uri = "https://test.com/api/password"
        };

        // Assert
        passwordService.IsEnabled.Should().BeTrue();
        passwordService.Uri.Should().Be("https://test.com/api/password");
    }

    [Fact]
    public void PasswordService_DefaultValues_ShouldBeCorrect()
    {
        // Arrange
        var passwordService = new PasswordService();

        // Assert
        passwordService.IsEnabled.Should().BeFalse();
        passwordService.Uri.Should().BeNull();
    }

    [Theory]
    [InlineData(true, "https://api.example.com")]
    [InlineData(false, "https://test.example.com")]
    [InlineData(true, "")]
    [InlineData(false, null)]
    public void PasswordService_WithDifferentValues_ShouldSetCorrectly(bool isEnabled, string uri)
    {
        // Arrange
        var passwordService = new PasswordService
        {
            IsEnabled = isEnabled,
            Uri = uri
        };

        // Assert
        passwordService.IsEnabled.Should().Be(isEnabled);
        passwordService.Uri.Should().Be(uri);
    }
}
