using AlphaPlus.CashService.API.Features.Payments.Models;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Models
{
    public class ProviderModelTests
    {
        [Fact]
        public void Constructor_WhenCalled_InitializesPropertiesWithDefaultValues()
        {
            // Arrange & Act
            var provider = new ProviderModel();

            // Assert
            Assert.Equal(0, provider.Id);
            Assert.Equal(string.Empty, provider.Name);
            Assert.Equal(0, provider.CustId);
            Assert.Equal(0, provider.Status);
            Assert.NotNull(provider.ProviderSettings);
            Assert.Empty(provider.ProviderSettings);
            Assert.True(provider.LastUpdated <= DateTime.UtcNow);
        }

        [Fact]
        public void LastUpdatedStr_WhenPropertyAccessed_ReturnsFormattedDateTime()
        {
            // Arrange
            var provider = new ProviderModel();

            // Act
            var result = provider.LastUpdatedStr;

            // Assert
            Assert.Matches(@"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2} [AP]M$", result);
        }

        [Fact]
        public void Properties_WhenSet_RetainAssignedValues()
        {
            // Arrange
            var provider = new ProviderModel();
            var testId = 123;
            var testName = "Test Provider";
            var testCustId = 456;
            var testStatus = 1;
            var testSettings = new List<ProviderSettingsModel> { new ProviderSettingsModel() };

            // Act
            provider.Id = testId;
            provider.Name = testName;
            provider.CustId = testCustId;
            provider.Status = testStatus;
            provider.ProviderSettings = testSettings;

            // Assert
            Assert.Equal(testId, provider.Id);
            Assert.Equal(testName, provider.Name);
            Assert.Equal(testCustId, provider.CustId);
            Assert.Equal(testStatus, provider.Status);
            Assert.Same(testSettings, provider.ProviderSettings);
        }
    }
} 