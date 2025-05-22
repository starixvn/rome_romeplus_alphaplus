using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.RemotingRef;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Services;

public class CacheRefServiceTests
{
    private readonly Mock<IRemotingRefService> _remotingRefServiceMock;
    private readonly ICacheRefService _cacheRefService;
    private const string DefaultLanguageCode = "en-US";

    public CacheRefServiceTests()
    {
        _remotingRefServiceMock = new Mock<IRemotingRefService>();
        _cacheRefService = new CacheRefService(_remotingRefServiceMock.Object);
    }

    [Fact]
    public void GetProviderName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const int providerId = 1;
        const string defaultName = "Default Provider";
        const string expectedName = "Test Provider";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashPaymentProvider",
                providerId.ToString(),
                DefaultLanguageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetProviderName(providerId, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetProviderName_WhenResourceDataIsEmpty_ReturnsDefaultName()
    {
        // Arrange
        const int providerId = 1;
        const string defaultName = "Default Provider";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashPaymentProvider",
                providerId.ToString(),
                DefaultLanguageCode,
                string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetProviderName(providerId, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetPaymentMethodName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const int methodId = 1;
        const string defaultName = "Default Method";
        const string expectedName = "Test Method";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashPaymentMethod",
                methodId.ToString(),
                DefaultLanguageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetPaymentMethodName(methodId, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetPaymentMethodName_WhenResourceDataIsEmpty_ReturnsDefaultName()
    {
        // Arrange
        const int methodId = 1;
        const string defaultName = "Default Method";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashPaymentMethod",
                methodId.ToString(),
                DefaultLanguageCode,
                string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetPaymentMethodName(methodId, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetBankName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const int bankId = 1;
        const string defaultName = "Default Bank";
        const string expectedName = "Test Bank";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashBankList",
                bankId.ToString(),
                DefaultLanguageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetBankName(bankId, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetBankName_WhenResourceDataIsEmpty_ReturnsDefaultName()
    {
        // Arrange
        const int bankId = 1;
        const string defaultName = "Default Bank";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashBankList",
                bankId.ToString(),
                DefaultLanguageCode,
                string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetBankName(bankId, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetDepositAutoModeName_WhenResourceDataExists_ReturnsResourceData()
    {
        // Arrange
        const string sourceType = "test";
        const string defaultName = "Default Mode";
        const string expectedName = "Test Mode";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashDepositAutoMode",
                sourceType,
                DefaultLanguageCode,
                string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetDepositAutoModeName(sourceType, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetDepositAutoModeName_WhenResourceDataIsEmpty_ReturnsDefaultName()
    {
        // Arrange
        const string sourceType = "test";
        const string defaultName = "Default Mode";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "CashDepositAutoMode",
                sourceType,
                DefaultLanguageCode,
                string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetDepositAutoModeName(sourceType, defaultName, DefaultLanguageCode);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetResourceDataByRefIdAndResourceId_WhenCalledWithIntResourceId_ReturnsResourceData()
    {
        // Arrange
        const string refId = "TestRef";
        const int resourceId = 1;
        const string expectedData = "Test Data";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                DefaultLanguageCode,
                string.Empty))
            .Returns(expectedData);

        // Act
        var result = _cacheRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, DefaultLanguageCode);

        // Assert
        Assert.Equal(expectedData, result);
    }
} 