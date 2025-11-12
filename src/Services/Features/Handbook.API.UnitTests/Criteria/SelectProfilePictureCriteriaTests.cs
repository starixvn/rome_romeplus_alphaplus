using AlphaPlus.Features.Handbook.API.Criteria;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Criteria
{
    public class SelectProfilePictureCriteriaTests
    {
        [Fact]
        public void Constructor_SetsActorIdCorrectly()
        {
            // Arrange
            var actorId = 123;

            // Act
            var criteria = new SelectProfilePictureCriteria(actorId);

            // Assert
            Assert.Equal(actorId, criteria.ActorId);
        }

        [Fact]
        public void GetSettingKey_ReturnsCorrectValue()
        {
            // Arrange
            var criteria = new SelectProfilePictureCriteria(1);

            // Act
            var result = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Age_CustomerSelProfilePicture", result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(100, true)]
        [InlineData(int.MaxValue, true)]
        public void IsValid_WithPositiveActorId_ReturnsTrue(int actorId, bool expected)
        {
            // Arrange
            var criteria = new SelectProfilePictureCriteria(actorId);

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
        public void IsValid_WithZeroOrNegativeActorId_ReturnsFalse(int actorId, bool expected)
        {
            // Arrange
            var criteria = new SelectProfilePictureCriteria(actorId);

            // Act
            var result = criteria.IsValid();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ActorId_CanBeRead()
        {
            // Arrange
            var actorId = 456;
            var criteria = new SelectProfilePictureCriteria(actorId);

            // Act & Assert
            Assert.Equal(actorId, criteria.ActorId);
            
            // Verify the property can be read
            var property = typeof(SelectProfilePictureCriteria).GetProperty(nameof(SelectProfilePictureCriteria.ActorId));
            Assert.NotNull(property);
            Assert.True(property.CanRead);
        }

        [Fact]
        public void Constructor_WithDifferentValues_CreatesIndependentInstances()
        {
            // Arrange & Act
            var criteria1 = new SelectProfilePictureCriteria(100);
            var criteria2 = new SelectProfilePictureCriteria(200);

            // Assert
            Assert.Equal(100, criteria1.ActorId);
            Assert.Equal(200, criteria2.ActorId);
            Assert.NotEqual(criteria1.ActorId, criteria2.ActorId);
        }

        [Fact]
        public void GetSettingKey_IsConsistentAcrossInstances()
        {
            // Arrange
            var criteria1 = new SelectProfilePictureCriteria(100);
            var criteria2 = new SelectProfilePictureCriteria(200);

            // Act
            var settingKey1 = criteria1.GetSettingKey();
            var settingKey2 = criteria2.GetSettingKey();

            // Assert
            Assert.Equal(settingKey1, settingKey2);
            Assert.Equal("Age_CustomerSelProfilePicture", settingKey1);
            Assert.Equal("Age_CustomerSelProfilePicture", settingKey2);
        }

        [Fact]
        public void IsValid_IsConsistentForSameActorId()
        {
            // Arrange
            var actorId = 123;
            var criteria = new SelectProfilePictureCriteria(actorId);

            // Act
            var result1 = criteria.IsValid();
            var result2 = criteria.IsValid();

            // Assert
            Assert.Equal(result1, result2);
            Assert.True(result1); // Should be valid for positive actorId
        }

        [Fact]
        public void Criteria_InheritsFromCriteriaBase()
        {
            // Arrange
            var criteria = new SelectProfilePictureCriteria(1);

            // Act & Assert
            Assert.IsAssignableFrom<Fanex.Data.Repository.CriteriaBase>(criteria);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(42)]
        [InlineData(999)]
        [InlineData(1000000)]
        public void Constructor_WithValidValues_DoesNotThrow(int actorId)
        {
            // Act & Assert - Should not throw any exception
            var criteria = new SelectProfilePictureCriteria(actorId);
            Assert.Equal(actorId, criteria.ActorId);
        }
    }
}
