namespace AgencyCash.API.Shared.Helpers.Tests;

[Trait("Category", "Shared")]
public class DomainHelperTests
{
    [Fact]
    public void RemoveProtocol_WhenNullDomain_ReturnsNull()
    {
        // Act
        var result = DomainHelper.RemoveProtocol(null);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void RemoveProtocol_WhenEmptyDomain_ReturnsEmpty()
    {
        // Act
        var result = DomainHelper.RemoveProtocol(string.Empty);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void RemoveProtocol_WhenWhitespaceDomain_ReturnsWhitespace()
    {
        // Act
        var result = DomainHelper.RemoveProtocol("   ");

        // Assert
        Assert.Equal("   ", result);
    }

    [Fact]
    public void RemoveProtocol_WhenHttpProtocol_RemovesProtocol()
    {
        // Arrange
        var domain = "http://example.com";

        // Act
        var result = DomainHelper.RemoveProtocol(domain);

        // Assert
        Assert.Equal("example.com", result);
    }

    [Fact]
    public void RemoveProtocol_WhenHttpsProtocol_RemovesProtocol()
    {
        // Arrange
        var domain = "https://example.com";

        // Act
        var result = DomainHelper.RemoveProtocol(domain);

        // Assert
        Assert.Equal("example.com", result);
    }

    [Fact]
    public void RemoveProtocol_WhenNoProtocol_ReturnsSameDomain()
    {
        // Arrange
        var domain = "example.com";

        // Act
        var result = DomainHelper.RemoveProtocol(domain);

        // Assert
        Assert.Equal("example.com", result);
    }

    [Fact]
    public void RemoveProtocol_WithMixedCaseProtocol_RemovesProtocol()
    {
        // Arrange
        var domain = "HttP://example.com";

        // Act
        var result = DomainHelper.RemoveProtocol(domain);

        // Assert
        Assert.Equal("example.com", result);
    }
}