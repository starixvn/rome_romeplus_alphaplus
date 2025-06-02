using AlphaPlus.CashService.API.Features.Payments.Criteria;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Payments.Criteria
{
    public class GetDailyLimitCriteriaTests
    {
        [Fact]
        public void Constructor_WhenValidParametersProvided_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            int custId = 1;
            int modeView = 2;

            // Act
            var criteria = new GetDailyLimitCriteria(custId, modeView);

            // Assert
            Assert.Equal(custId, criteria.CustId);
            Assert.Equal(modeView, criteria.ModeView);
        }

        [Fact]
        public void GetSettingKey_WhenCalled_ShouldReturnCorrectKey()
        {
            // Arrange
            var criteria = new GetDailyLimitCriteria(1, 2);

            // Act
            var settingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CashService_AgentBankAccountLimit_Sel", settingKey);
        }

        [Theory]
        [InlineData(1, true)]     // Valid case
        [InlineData(0, false)]    // Invalid case
        [InlineData(-1, false)]   // Invalid case
        public void IsValid_WhenCalled_ShouldReturnExpectedResult(int custId, bool expectedResult)
        {
            // Arrange
            var criteria = new GetDailyLimitCriteria(custId, 2);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedResult, isValid);
        }

        [Fact]
        public void ModeView_WhenSet_ShouldUpdateValue()
        {
            // Arrange
            var criteria = new GetDailyLimitCriteria(1, 2);
            var newModeView = 3;

            // Act
            criteria.ModeView = newModeView;

            // Assert
            Assert.Equal(newModeView, criteria.ModeView);
        }
    }
} 