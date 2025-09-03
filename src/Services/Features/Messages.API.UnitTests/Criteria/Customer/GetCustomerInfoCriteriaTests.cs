using AlphaPlus.Features.Messages.API.Criteria.Customer;
using Fanex.Data.Repository;
using Xunit;

namespace AlphaPlus.Features.Messages.API.UnitTests.Criteria.Customer
{
    public class GetCustomerInfoCriteriaTests
    {
        [Fact]
        public void Constructor_SetsCustIdCorrectly()
        {
            // Arrange
            var custId = 123;

            // Act
            var criteria = new GetCustomerInfoCriteria(custId);

            // Assert
            Assert.Equal(custId, criteria.CustId);
        }

        [Fact]
        public void GetSettingKey_ReturnsCorrectValue()
        {
            // Arrange
            var criteria = new GetCustomerInfoCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_GetCustomerInfo", result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_WithPositiveCustId_ReturnsTrue(int custId, bool expected)
        {
            // Arrange
            var criteria = new GetCustomerInfoCriteria(custId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        [InlineData(-100, false)]
        [InlineData(int.MinValue, false)]
        public void IsValid_WithZeroOrNegativeCustId_ReturnsFalse(int custId, bool expected)
        {
            // Arrange
            var criteria = new GetCustomerInfoCriteria(custId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CustId_IsReadOnly()
        {
            // Arrange
            var custId = 456;
            var criteria = new GetCustomerInfoCriteria(custId);

            // Act & Assert
            Assert.Equal(custId, criteria.CustId);
            
            // Verify the property doesn't have a setter
            var property = typeof(GetCustomerInfoCriteria).GetProperty(nameof(criteria.CustId));
            Assert.NotNull(property);
            Assert.True(property.CanRead);
            Assert.False(property.CanWrite);
        }

        [Fact]
        public void Constructor_WithDifferentValues_CreatesIndependentInstances()
        {
            // Arrange & Act
            var criteria1 = new GetCustomerInfoCriteria(100);
            var criteria2 = new GetCustomerInfoCriteria(200);

            // Assert
            Assert.Equal(100, criteria1.CustId);
            Assert.Equal(200, criteria2.CustId);
            Assert.NotEqual(criteria1.CustId, criteria2.CustId);
        }

        [Fact]
        public void GetSettingKey_IsConsistentAcrossInstances()
        {
            // Arrange
            var criteria1 = new GetCustomerInfoCriteria(100);
            var criteria2 = new GetCustomerInfoCriteria(200);

            // Act
            var settingKey1 = criteria1.GetSettingKey();
            var settingKey2 = criteria2.GetSettingKey();

            // Assert
            Assert.Equal(settingKey1, settingKey2);
            Assert.Equal("Age_GetCustomerInfo", settingKey1);
            Assert.Equal("Age_GetCustomerInfo", settingKey2);
        }

        [Fact]
        public void IsValid_IsConsistentForSameCustId()
        {
            // Arrange
            var custId = 123;
            var criteria = new GetCustomerInfoCriteria(custId);

            // Act
            var result1 = criteria.IsValid();
            var result2 = criteria.IsValid();

            // Assert
            Assert.Equal(result1, result2);
            Assert.True(result1); // Should be valid for positive custId
        }

        [Fact]
        public void Criteria_InheritsFromCriteriaBase()
        {
            // Arrange
            var criteria = new GetCustomerInfoCriteria(1);

            // Act & Assert
            Assert.IsAssignableFrom<CriteriaBase>(criteria);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(42)]
        [InlineData(999)]
        [InlineData(1000000)]
        public void Constructor_WithValidValues_DoesNotThrow(int custId)
        {
            // Act & Assert - Should not throw any exception
            var criteria = new GetCustomerInfoCriteria(custId);
            Assert.Equal(custId, criteria.CustId);
        }

        [Fact]
        public void Constructor_WithZero_DoesNotThrow()
        {
            // Act & Assert - Should not throw any exception even with invalid value
            var criteria = new GetCustomerInfoCriteria(0);
            Assert.Equal(0, criteria.CustId);
            Assert.False(criteria.IsValid());
        }

        [Fact]
        public void Constructor_WithNegativeValue_DoesNotThrow()
        {
            // Act & Assert - Should not throw any exception even with invalid value
            var criteria = new GetCustomerInfoCriteria(-1);
            Assert.Equal(-1, criteria.CustId);
            Assert.False(criteria.IsValid());
        }
    }
}
