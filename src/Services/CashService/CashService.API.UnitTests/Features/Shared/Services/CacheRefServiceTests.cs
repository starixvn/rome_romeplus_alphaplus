using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.Logging;
using Fanex.RemotingRef;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Services;

public class CacheRefServiceTests
{
    private readonly Mock<IRemotingRefService> _remotingRefServiceMock;
    private readonly Mock<ILogger> _loggerMock;
    private readonly CacheRefService _sut;

    public CacheRefServiceTests()
    {
        _remotingRefServiceMock = new Mock<IRemotingRefService>();
        _loggerMock = new Mock<ILogger>();
        _sut = new CacheRefService(_remotingRefServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void GetProviderName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const int providerId = 1;
        const string defaultName = "Default Provider";
        const string languageCode = "en";
        const string expectedName = "Test Provider";

        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashPaymentProvider",
                providerId.ToString(),
                languageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _sut.GetProviderName(providerId, defaultName, languageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetProviderName_WhenResourceDataDoesNotExist_ReturnsDefaultName()
    {
        // Arrange
        const int providerId = 1;
        const string defaultName = "Default Provider";
        const string languageCode = "en";

        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashPaymentProvider",
                providerId.ToString(),
                languageCode,
                string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _sut.GetProviderName(providerId, defaultName, languageCode);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetPaymentMethodName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const int methodId = 1;
        const string defaultName = "Default Method";
        const string languageCode = "en";
        const string expectedName = "Test Method";

        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashPaymentMethod",
                methodId.ToString(),
                languageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _sut.GetPaymentMethodName(methodId, defaultName, languageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetBankName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const int bankId = 1;
        const string defaultName = "Default Bank";
        const string languageCode = "en";
        const string expectedName = "Test Bank";

        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashBankList",
                bankId.ToString(),
                languageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _sut.GetBankName(bankId, defaultName, languageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetDepositAutoModeName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const string sourceType = "AUTO";
        const string defaultName = "Default Mode";
        const string languageCode = "en";
        const string expectedName = "Test Mode";

        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashDepositAutoMode",
                sourceType,
                languageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _sut.GetDepositAutoModeName(sourceType, defaultName, languageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetResourceDataByRefIdAndResourceId_WhenExceptionOccurs_ReturnsEmptyString()
    {
        // Arrange
        const string refId = "TestRef";
        const int resourceId = 1;
        const string languageCode = "en";

        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                languageCode,
                string.Empty))
            .Throws(new Exception("Test exception"));

        // Act
        var result = _sut.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode);

        // Assert
        Assert.Equal(string.Empty, result);
        _loggerMock.Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
    }
} 