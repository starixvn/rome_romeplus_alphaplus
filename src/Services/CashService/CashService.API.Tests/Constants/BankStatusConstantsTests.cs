using AlphaPlus.CashService.API.Constants;
using Xunit;

namespace AlphaPlus.CashService.API.Tests.Constants
{
    public class BankStatusConstantsTests
    {
        [Fact]
        public void BankStatusConstants_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal(1, BankStatusConstants.Active);
            Assert.Equal(2, BankStatusConstants.Inactive);
            Assert.Equal(3, BankStatusConstants.Suspended);
            Assert.Equal(4, BankStatusConstants.Blocked);
            Assert.Equal(5, BankStatusConstants.UnderMaintenance);
            Assert.Equal(6, BankStatusConstants.PendingVerification);
            Assert.Equal(7, BankStatusConstants.Deactivated);
            Assert.Equal(8, BankStatusConstants.Restricted);
        }

        [Fact]
        public void BankStatusConstants_ShouldHaveUniqueValues()
        {
            // Arrange
            var allStatuses = new[]
            {
                BankStatusConstants.Active,
                BankStatusConstants.Inactive,
                BankStatusConstants.Suspended,
                BankStatusConstants.Blocked,
                BankStatusConstants.UnderMaintenance,
                BankStatusConstants.PendingVerification,
                BankStatusConstants.Deactivated,
                BankStatusConstants.Restricted
            };

            // Assert
            Assert.Equal(allStatuses.Length, allStatuses.Distinct().Count());
        }
    }
} 