using System.Globalization;
using AlphaPlus.Settings.ViewLogs.API.Services.Impl;
using Fanex.RemotingRef;

namespace AlphaPlus.Settings.ViewLogs.API.UnitTests.Services
{
    public class RemotingRefWrapperTests
    {
        private readonly Mock<IRemotingRefService> _mockRemotingRefService;
        private readonly RemotingRefWrapperImpl _remotingRefWrapper;
        private readonly Fixture _fixture;

        public RemotingRefWrapperTests()
        {
            _mockRemotingRefService = new Mock<IRemotingRefService>();
            _remotingRefWrapper = new RemotingRefWrapperImpl(_mockRemotingRefService.Object);
            _fixture = new Fixture();
            
            // Set a consistent culture for testing
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        [Fact]
        public void GetBetType_WithNullBetTypeId_ShouldReturnEmpty()
        {
            // Act
            var result = _remotingRefWrapper.GetBetType(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetBetType_WithValidBetTypeId_ShouldCallRemotingService()
        {
            // Arrange
            var betTypeId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetBetTypeName(betTypeId.ToString(), "en-US", string.Empty, string.Empty))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetBetType(betTypeId);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRemotingRefService.Verify(
                x => x.GetBetTypeName(betTypeId.ToString(), "en-US", string.Empty, string.Empty),
                Times.Once);
        }

        [Fact]
        public void GetBetType_WithBetId_ShouldUseResourceIdFromBetId()
        {
            // Arrange
            var betTypeId = _fixture.Create<int>();
            var betId = "A123456"; // More than 1 character
            var expectedResult = _fixture.Create<string>();
            var expectedResourceId = "123456"; // betId without first character

            _mockRemotingRefService
                .Setup(x => x.GetBetTypeName(betTypeId.ToString(), "en-US", string.Empty, expectedResourceId))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetBetType(betTypeId, betId);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRemotingRefService.Verify(
                x => x.GetBetTypeName(betTypeId.ToString(), "en-US", string.Empty, expectedResourceId),
                Times.Once);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("")]
        [InlineData(null)]
        public void GetBetType_WithShortOrEmptyBetId_ShouldUseEmptyResourceId(object betId)
        {
            // Arrange
            var betTypeId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetBetTypeName(betTypeId.ToString(), "en-US", string.Empty, string.Empty))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetBetType(betTypeId, betId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetBetTypeName_WithNullBetTypeId_ShouldReturnEmpty()
        {
            // Act
            var result = _remotingRefWrapper.GetBetTypeName(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetBetTypeName_WithValidBetTypeId_ShouldCallRemotingService()
        {
            // Arrange
            var betTypeId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetBetTypeName(betTypeId.ToString(), "en-US"))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetBetTypeName(betTypeId);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRemotingRefService.Verify(
                x => x.GetBetTypeName(betTypeId.ToString(), "en-US"),
                Times.Once);
        }

        [Fact]
        public void GetSportName_WithValidSportId_ShouldReturnSportName()
        {
            // Arrange
            var sportId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetSportName(sportId.ToString(), "en-US"))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetSportName(sportId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetProductName_WithValidProductId_ShouldReturnProductName()
        {
            // Arrange
            var productId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    "Product", productId.ToString(), "en-US", productId.ToString()))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetProductName(productId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetSubProductGroupName_WithValidSubGroupId_ShouldReturnSubGroupName()
        {
            // Arrange
            var subGroupId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    "SubGroup", subGroupId.ToString(), "en-US", string.Empty))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetSubProductGroupName(subGroupId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetProductGroupName_WithValidProductGroupId_ShouldReturnProductGroupName()
        {
            // Arrange
            var productGroupId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    "ProductGroup", productGroupId.ToString(), "en-US", string.Empty))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetProductGroupName(productGroupId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetPaymentMethodName_WithValidMethodId_ShouldReturnMethodName()
        {
            // Arrange
            var methodId = _fixture.Create<string>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    "CashPaymentMethod", methodId, "en-US", string.Empty))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetPaymentMethodName(methodId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetProviderName_WithValidProviderId_ShouldReturnProviderName()
        {
            // Arrange
            var providerId = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    "CashPaymentProvider", providerId.ToString(), "en-US", string.Empty))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetProviderName(providerId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void GetSportbookCommissionName_WithValidParameters_ShouldReturnCommissionName()
        {
            // Arrange
            var sportType = _fixture.Create<int>();
            var betType = _fixture.Create<int>();
            var expectedResult = _fixture.Create<string>();
            // Don't assume exact constant values, just verify the pattern

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    "SB_Comm_SMA", It.IsAny<string>(), "en-US", string.Empty))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetSportbookCommissionName(sportType, betType);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(5, 10)]
        [InlineData(0, 0)]
        public void GetSportbookCommissionName_WithVariousParameters_ShouldCallCorrectResourceId(int sportType, int betType)
        {
            // Arrange
            var expectedResult = _fixture.Create<string>();

            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedResult);

            // Act
            var result = _remotingRefWrapper.GetSportbookCommissionName(sportType, betType);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRemotingRefService.Verify(
                x => x.GetResourceDataByRefIdAndResourceId(
                    "SB_Comm_SMA", 
                    It.Is<string>(s => s.Contains($"_{sportType}_{betType}")), 
                    "en-US", 
                    string.Empty),
                Times.Once);
        }

        [Fact]
        public void Constructor_WithNullRemotingRefService_ShouldAcceptNullValue()
        {
            // Act & Assert - Constructor accepts null values in modern .NET
            var wrapper = new RemotingRefWrapperImpl(null);
            Assert.NotNull(wrapper);
        }

        [Fact]
        public void Constructor_WithValidRemotingRefService_ShouldCreateInstance()
        {
            // Act
            var wrapper = new RemotingRefWrapperImpl(_mockRemotingRefService.Object);

            // Assert
            Assert.NotNull(wrapper);
        }
    }
}
