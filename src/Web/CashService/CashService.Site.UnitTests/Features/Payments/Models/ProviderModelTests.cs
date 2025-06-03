using AlphaPlus.CashService.Site.Features.Payments.Models;
using Xunit;

namespace AlphaPlus.CashService.Site.UnitTests.Features.Payments.Models
{
    public class ProviderModelTests
    {
        [Fact]
        public void Constructor_WhenCalled_InitializesPropertiesWithDefaultValues()
        {
            // Arrange & Act
            var provider = new ProviderModel();

            // Assert
            Assert.Equal(string.Empty, provider.Name);
            Assert.Equal(0, provider.Status);
            Assert.NotNull(provider.ProviderSettings);
            Assert.Empty(provider.ProviderSettings);
            Assert.True(provider.LastUpdated <= DateTime.UtcNow);
        }

        [Fact]
        public void LastUpdatedStr_WhenCalled_ReturnsFormattedDateTime()
        {
            // Arrange
            var provider = new ProviderModel
            {
                LastUpdated = new DateTime(2024, 3, 15, 14, 30, 0, DateTimeKind.Utc)
            };

            // Act
            var result = provider.LastUpdatedStr;

            // Assert
            Assert.Equal("03/15/2024 02:30:00 PM", result);
        }

        [Fact]
        public void Properties_WhenSet_RetainAssignedValues()
        {
            // Arrange
            var provider = new ProviderModel();
            var settings = new List<ProviderSettingsModel>();

            // Act
            provider.Id = 1;
            provider.Name = "Test Provider";
            provider.CustId = 100;
            provider.Status = 1;
            provider.ProviderSettings = settings;

            // Assert
            Assert.Equal(1, provider.Id);
            Assert.Equal("Test Provider", provider.Name);
            Assert.Equal(100, provider.CustId);
            Assert.Equal(1, provider.Status);
            Assert.Same(settings, provider.ProviderSettings);
        }
    }
} 