using Xunit;
using AlphaPlus.CashService.API.Criteria.Payments;

namespace AlphaPlus.CashService.API.UnitTests.Criteria.Payments
{
    public class GetTransactionTypesCriteriaTests
    {
        private readonly GetTransactionTypesCriteria _criteria;

        public GetTransactionTypesCriteriaTests()
        {
            _criteria = new GetTransactionTypesCriteria();
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnExpectedKey()
        {
            // Act
            var result = _criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_TransTypeControl", result);
        }

        [Fact]
        public void IsValid_WhenCalled_ShouldReturnTrue()
        {
            // Act
            var result = _criteria.IsValid();

            // Assert
            Assert.True(result);
        }
    }
} 