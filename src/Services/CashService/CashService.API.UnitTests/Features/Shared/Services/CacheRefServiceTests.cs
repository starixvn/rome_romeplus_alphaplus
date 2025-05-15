using AlphaPlus.CashService.API.Features.Shared.Services;
using Fanex.RemotingRef;
using Moq;
using Xunit;

namespace AlphaPlus.CashService.API.UnitTests.Features.Shared.Services;

public class CacheRefServiceTests
{
    private readonly Mock<IRemotingRefService> _remotingRefServiceMock;
    private readonly ICacheRefService _cacheRefService;

    public CacheRefServiceTests()
    {
        _remotingRefServiceMock = new Mock<IRemotingRefService>();
        _cacheRefService = new CacheRefService(_remotingRefServiceMock.Object);
    }

    [Fact]
    public void GetProviderName_WhenProviderExists_ReturnsProviderName()
    {
        // Arrange
        const int providerId = 1;
        const string expectedName = "Test Provider";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashPaymentProvider",
            providerId.ToString(),
            It.IsAny<string>(),
            string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetProviderName(providerId, "Default");

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetProviderName_WhenProviderDoesNotExist_ReturnsDefaultName()
    {
        // Arrange
        const int providerId = 1;
        const string defaultName = "Default Provider";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashPaymentProvider",
            providerId.ToString(),
            It.IsAny<string>(),
            string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetProviderName(providerId, defaultName);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetPaymentMethodName_WhenMethodExists_ReturnsMethodName()
    {
        // Arrange
        const int methodId = 1;
        const string expectedName = "Test Method";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashPaymentMethod",
            methodId.ToString(),
            It.IsAny<string>(),
            string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetPaymentMethodName(methodId, "Default");

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetPaymentMethodName_WhenMethodDoesNotExist_ReturnsDefaultName()
    {
        // Arrange
        const int methodId = 1;
        const string defaultName = "Default Method";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashPaymentMethod",
            methodId.ToString(),
            It.IsAny<string>(),
            string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetPaymentMethodName(methodId, defaultName);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetBankName_WhenBankExists_ReturnsBankName()
    {
        // Arrange
        const int bankId = 1;
        const string expectedName = "Test Bank";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashBankList",
            bankId.ToString(),
            It.IsAny<string>(),
            string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetBankName(bankId, "Default");

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetBankName_WhenBankDoesNotExist_ReturnsDefaultName()
    {
        // Arrange
        const int bankId = 1;
        const string defaultName = "Default Bank";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashBankList",
            bankId.ToString(),
            It.IsAny<string>(),
            string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetBankName(bankId, defaultName);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetDepositAutoModeName_WhenModeExists_ReturnsModeName()
    {
        // Arrange
        const string sourceType = "test";
        const string expectedName = "Test Mode";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashDepositAutoMode",
            sourceType,
            It.IsAny<string>(),
            string.Empty))
            .Returns(expectedName);

        // Act
        var result = _cacheRefService.GetDepositAutoModeName(sourceType, "Default");

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Fact]
    public void GetDepositAutoModeName_WhenModeDoesNotExist_ReturnsDefaultName()
    {
        // Arrange
        const string sourceType = "test";
        const string defaultName = "Default Mode";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            "CashDepositAutoMode",
            sourceType,
            It.IsAny<string>(),
            string.Empty))
            .Returns(string.Empty);

        // Act
        var result = _cacheRefService.GetDepositAutoModeName(sourceType, defaultName);

        // Assert
        Assert.Equal(defaultName, result);
    }

    [Fact]
    public void GetResourceDataByRefIdAndResourceId_WhenCalled_ReturnsResourceData()
    {
        // Arrange
        const string refId = "TestRef";
        const int resourceId = 1;
        const string expectedData = "Test Data";
        _remotingRefServiceMock.Setup(x => x.GetResourceDataByRefIdAndResourceId(
            refId,
            resourceId.ToString(),
            It.IsAny<string>(),
            string.Empty))
            .Returns(expectedData);

        // Act
        var result = _cacheRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId);

        // Assert
        Assert.Equal(expectedData, result);
    }
} 