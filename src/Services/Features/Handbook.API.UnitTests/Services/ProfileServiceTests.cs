using AlphaPlus.Features.Handbook.API.Criteria;
using AlphaPlus.Features.Handbook.API.Models;
using AlphaPlus.Features.Handbook.API.Services;
using AutoFixture;
using Fanex.Data.Repository;
using Moq;
using Xunit;

namespace AlphaPlus.Features.Handbook.API.UnitTests.Services
{
    public class ProfileServiceTests
    {
        [Fact]
        public void Constructor_WithValidRepository_DoesNotThrow()
        {
            // Arrange
            var mockRepository = new Mock<IDynamicRepository>();
            
            // Act & Assert - Should not throw exception
            var service = new ProfileService(mockRepository.Object);
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullRepository_DoesNotThrow()
        {
            // Act & Assert - ProfileService doesn't validate null repository in constructor
            var service = new ProfileService(null);
            Assert.NotNull(service);
        }

        [Fact]
        public void GetProfilePicture_ReturnsNonNullResult()
        {
            // Arrange
            var mockRepository = new Mock<IDynamicRepository>();
            var service = new ProfileService(mockRepository.Object);
            var custId = 123;

            // Act
            var result = service.GetProfilePicture(custId);

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-1)]
        public void GetProfilePicture_WithDifferentCustIds_ReturnsNonNullResult(int custId)
        {
            // Arrange
            var mockRepository = new Mock<IDynamicRepository>();
            var service = new ProfileService(mockRepository.Object);

            // Act
            var result = service.GetProfilePicture(custId);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetProfilePicture_ReturnsProfilePictureModelInstance()
        {
            // Arrange
            var mockRepository = new Mock<IDynamicRepository>();
            var service = new ProfileService(mockRepository.Object);
            var custId = 123;

            // Act
            var result = service.GetProfilePicture(custId);

            // Assert
            Assert.IsType<ProfilePictureModel>(result);
        }
    }
}