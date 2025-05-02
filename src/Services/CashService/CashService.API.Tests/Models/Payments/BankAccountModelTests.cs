using System;
using Xunit;
using AlphaPlus.CashService.API.Models.Payments;

namespace AlphaPlus.CashService.API.Tests.Models.Payments
{
    public class BankAccountModelTests
    {
        [Fact]
        public void LastUpdatedStr_WhenDateTimeIsSet_ReturnsFormattedDateTime()
        {
            // Arrange
            var bankAccount = new BankAccountModel
            {
                LastUpdated = new DateTime(2024, 3, 15, 14, 30, 45, DateTimeKind.Utc)
            };

            // Act
            string result = bankAccount.LastUpdatedStr;

            // Assert
            Assert.Equal("03/15/2024 02:30:45 PM", result);
        }

        [Fact]
        public void LastUpdatedStr_WhenUsingDefaultConstructor_ReturnsCurrentTimeFormatted()
        {
            // Arrange
            var bankAccount = new BankAccountModel();
            
            // Act
            string result = bankAccount.LastUpdatedStr;

            // Assert
            // Since the constructor sets LastUpdated to DateTime.UtcNow, we'll verify the format
            // rather than the exact time
            Assert.Matches(@"^\d{2}/\d{2}/\d{4} \d{2}:\d{2}:\d{2} [AP]M$", result);
        }

        [Theory]
        [InlineData(2024, 1, 1, 0, 0, 0, "01/01/2024 12:00:00 AM")]
        [InlineData(2024, 12, 31, 23, 59, 59, "12/31/2024 11:59:59 PM")]
        [InlineData(2024, 6, 15, 12, 0, 0, "06/15/2024 12:00:00 PM")]
        public void LastUpdatedStr_WhenVariousDatesProvided_ReturnsCorrectFormat(
            int year, int month, int day, 
            int hour, int minute, int second,
            string expected)
        {
            // Arrange
            var bankAccount = new BankAccountModel
            {
                LastUpdated = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc)
            };

            // Act
            string result = bankAccount.LastUpdatedStr;

            // Assert
            Assert.Equal(expected, result);
        }
    }
} 