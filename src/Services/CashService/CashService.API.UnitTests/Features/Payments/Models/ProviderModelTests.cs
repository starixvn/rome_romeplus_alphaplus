using System;
using Xunit;
using AlphaPlus.CashService.API.Features.Payments.Models;

namespace AlphaPlus.CashService.API.UnitTests.Models.Payments
{
    public class ProviderModelTests
    {
        [Fact]
        public void LastUpdatedStr_WhenLastUpdatedIsSet_ReturnsFormattedDateTime()
        {
            // Arrange
            var testDate = new DateTime(2024, 3, 15, 14, 30, 45, DateTimeKind.Utc);
            var provider = new ProviderModel
            {
                LastUpdated = testDate
            };

            // Act
            var result = provider.LastUpdatedStr;

            // Assert
            Assert.Equal("03/15/2024 02:30:45 PM", result);
        }
    }
} 