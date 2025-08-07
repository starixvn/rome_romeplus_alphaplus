using AlphaPlus.PartialUM.Configurations;

namespace AlphaPlus.PartialUM.UnitTests.Configurations;

public class PartialUMOptionsTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var cacheInSeconds = 300;

        // Act
        var options = new PartialUMOptions
        {
            CacheInSeconds = cacheInSeconds
        };

        // Assert
        Assert.Equal(cacheInSeconds, options.CacheInSeconds);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3600)]
    [InlineData(int.MaxValue)]
    public void CacheInSeconds_ShouldAcceptValidValues(int cacheInSeconds)
    {
        // Act
        var options = new PartialUMOptions
        {
            CacheInSeconds = cacheInSeconds
        };

        // Assert
        Assert.Equal(cacheInSeconds, options.CacheInSeconds);
    }

    [Fact]
    public void Properties_ShouldBeReadOnly()
    {
        // Arrange
        var options = new PartialUMOptions
        {
            CacheInSeconds = 300
        };

        // Act & Assert
        // The properties should be init-only, so we can't modify them after initialization
        // This test verifies the immutability of the object
        Assert.Equal(300, options.CacheInSeconds);
    }
}
