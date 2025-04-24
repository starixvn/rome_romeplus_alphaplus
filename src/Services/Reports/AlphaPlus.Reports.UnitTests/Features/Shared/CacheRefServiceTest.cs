using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.RemotingRef;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.UnitTests.Features.Shared;

[Trait("Category", "Shared")]
public class CacheRefServiceTest
{
    private readonly Mock<IRemotingRefService> remotingRefServiceMock;
    private Mock<IOptions<AppSettings>> optionsMock;
    private CacheRefService service;
    private AppSettings appSettings;

    public CacheRefServiceTest()
    {
        remotingRefServiceMock = new Mock<IRemotingRefService>();
        optionsMock = new Mock<IOptions<AppSettings>>();
        appSettings = new AppSettings { SiteLabel = "777" };

        optionsMock.Setup(x => x.Value).Returns(appSettings);

        service = new CacheRefService(remotingRefServiceMock.Object, optionsMock.Object);
    }

    [Fact]
    public void GetSportName_ShouldCallRemotingRefService()
    {
        // Arrange
        const int sportType = 1;
        const string expectedSportName = "Football";

        remotingRefServiceMock
            .Setup(x => x.GetSportName(sportType.ToString(), It.IsAny<string>()))
            .Returns(expectedSportName);

        // Act
        var result = service.GetSportName(sportType);

        // Assert
        Assert.Equal(expectedSportName, result);
        remotingRefServiceMock.Verify(x => x.GetSportName(sportType.ToString(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetBetTypeName_ShouldCallRemotingRefService()
    {
        // Arrange
        const int betType = 1;
        const string expectedBetTypeName = "Moneyline";

        remotingRefServiceMock
            .Setup(x => x.GetBetTypeName(betType.ToString(), It.IsAny<string>()))
            .Returns(expectedBetTypeName);

        // Act
        var result = service.GetBetTypeName(betType);

        // Assert
        Assert.Equal(expectedBetTypeName, result);
        remotingRefServiceMock.Verify(x => x.GetBetTypeName(betType.ToString(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetLeagueName_SingleParameter_ShouldCallRemotingRefService()
    {
        // Arrange
        const int leagueId = 123;
        const string expectedLeagueName = "Premier League";

        remotingRefServiceMock
            .Setup(x => x.GetLeagueName(leagueId.ToString(), It.IsAny<string>()))
            .Returns(expectedLeagueName);

        // Act
        var result = service.GetLeagueName(leagueId);

        // Assert
        Assert.Equal(expectedLeagueName, result);
        remotingRefServiceMock.Verify(x => x.GetLeagueName(leagueId.ToString(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetLeagueName_WithGroupId_ShouldCallRemotingRefService()
    {
        // Arrange
        const int leagueId = 123;
        const int leagueGroupId = 456;
        const string expectedLeagueName = "Premier League";

        remotingRefServiceMock
            .Setup(x => x.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedLeagueName);

        // Act
        var result = service.GetLeagueName(leagueId, leagueGroupId);

        // Assert
        Assert.Equal(expectedLeagueName, result);
        remotingRefServiceMock.Verify(
            x => x.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void GetLeagueName3rd_ShouldCallRemotingRefService()
    {
        // Arrange
        const int league3rdId = 123;
        const string expectedLeagueName = "League 3rd";

        remotingRefServiceMock
            .Setup(x => x.GetLeagueName3rd(league3rdId.ToString(), It.IsAny<string>()))
            .Returns(expectedLeagueName);

        // Act
        var result = service.GetLeagueName3rd(league3rdId);

        // Assert
        Assert.Equal(expectedLeagueName, result);
        remotingRefServiceMock.Verify(x => x.GetLeagueName3rd(league3rdId.ToString(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetTeamName_ShouldCallRemotingRefService()
    {
        // Arrange
        const int teamId = 123;
        const string expectedTeamName = "Manchester United";

        remotingRefServiceMock
            .Setup(x => x.GetTeamName(teamId.ToString(), It.IsAny<string>()))
            .Returns(expectedTeamName);

        // Act
        var result = service.GetTeamName(teamId);

        // Assert
        Assert.Equal(expectedTeamName, result);
        remotingRefServiceMock.Verify(x => x.GetTeamName(teamId.ToString(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetTeamName3rd_ShouldCallRemotingRefService()
    {
        // Arrange
        const int team3rdId = 123;
        const string expectedTeamName = "Team 3rd";

        remotingRefServiceMock
            .Setup(x => x.GetTeamName3rd(team3rdId.ToString(), It.IsAny<string>()))
            .Returns(expectedTeamName);

        // Act
        var result = service.GetTeamName3rd(team3rdId);

        // Assert
        Assert.Equal(expectedTeamName, result);
        remotingRefServiceMock.Verify(x => x.GetTeamName3rd(team3rdId.ToString(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetOddsTypeName_ShouldCallRemotingRefService()
    {
        // Arrange
        const string oddsTypeId = "1";
        const string expectedOddsTypeName = "Decimal";

        remotingRefServiceMock
            .Setup(x => x.GetOddsTypeName(oddsTypeId, It.IsAny<string>()))
            .Returns(expectedOddsTypeName);

        // Act
        var result = service.GetOddsTypeName(oddsTypeId);

        // Assert
        Assert.Equal(expectedOddsTypeName, result);
        remotingRefServiceMock.Verify(x => x.GetOddsTypeName(oddsTypeId, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void GetProductName_IntOverload_ShouldCallStringOverload()
    {
        // Arrange
        const int productId = 1;
        const string expectedProductName = "Sport";

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedProductName);

        // Act
        var result = service.GetProductName(productId);

        // Assert
        Assert.Equal(expectedProductName, result);
    }

    [Fact]
    public void GetProductName_ShouldUseProductSabaRefId_WhenIsSiteLabel777IsTrue()
    {
        // Arrange
        const string productId = "1";
        const string expectedProductName = "Sport Saba";

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "Product_Saba",
                productId,
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedProductName);

        // Act
        var result = service.GetProductName(productId);

        // Assert
        Assert.Equal(expectedProductName, result);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("Product_Saba", productId, It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("Product", productId, It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public void GetProductName_ShouldFallbackToProductRefId_WhenProductSabaReturnsEmptyString()
    {
        // Arrange
        const string productId = "1";
        const string expectedProductName = "Sport";

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "Product_Saba",
                productId,
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(string.Empty);

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId,
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedProductName);

        // Act
        var result = service.GetProductName(productId);

        // Assert
        Assert.Equal(expectedProductName, result);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("Product_Saba", productId, It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("Product", productId, It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void GetProductName_ShouldUseProductRefId_WhenIsSiteLabel777IsFalse()
    {
        // Arrange
        const string productId = "1";
        const string expectedProductName = "Sport";

        optionsMock = new Mock<IOptions<AppSettings>>();
        appSettings = new AppSettings { SiteLabel = "ABC" };
        optionsMock.Setup(x => x.Value).Returns(appSettings);
        service = new CacheRefService(remotingRefServiceMock.Object, optionsMock.Object);

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId,
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedProductName);

        // Act
        var result = service.GetProductName(productId);

        // Assert
        Assert.Equal(expectedProductName, result);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("Product_Saba", productId, It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("Product", productId, It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void GetProductDetailName_ShouldUseProductDetailSabaRefId_WhenIsSiteLabel777IsTrue()
    {
        // Arrange
        const int productDetailId = 1;
        const string expectedProductDetailName = "Product Detail Saba";

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "ProductDetail_Saba",
                productDetailId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedProductDetailName);

        // Act
        var result = service.GetProductDetailName(productDetailId);

        // Assert
        Assert.Equal(expectedProductDetailName, result);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("ProductDetail_Saba", productDetailId.ToString(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("ProductDetail", productDetailId.ToString(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public void GetProductDetailName_ShouldFallbackToProductDetailRefId_WhenProductDetailSabaReturnsEmptyString()
    {
        // Arrange
        const int productDetailId = 1;
        const string expectedProductDetailName = "Product Detail";

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "ProductDetail_Saba",
                productDetailId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(string.Empty);

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "ProductDetail",
                productDetailId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedProductDetailName);

        // Act
        var result = service.GetProductDetailName(productDetailId);

        // Assert
        Assert.Equal(expectedProductDetailName, result);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("ProductDetail_Saba", productDetailId.ToString(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("ProductDetail", productDetailId.ToString(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void GetProductDetailName_ShouldUseProductDetailRefId_WhenIsSiteLabel777IsFalse()
    {
        // Arrange
        const int productDetailId = 1;
        const string expectedProductDetailName = "Product Detail";

        optionsMock = new Mock<IOptions<AppSettings>>();
        appSettings = new AppSettings { SiteLabel = "XYZ" };
        optionsMock.Setup(x => x.Value).Returns(appSettings);
        service = new CacheRefService(remotingRefServiceMock.Object, optionsMock.Object);

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                "ProductDetail",
                productDetailId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedProductDetailName);

        // Act
        var result = service.GetProductDetailName(productDetailId);

        // Assert
        Assert.Equal(expectedProductDetailName, result);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("ProductDetail_Saba", productDetailId.ToString(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId("ProductDetail", productDetailId.ToString(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public void GetResourceDataByRefIdAndResourceId_ShouldCallRemotingRefService()
    {
        // Arrange
        const string refId = "TestRefId";
        const int resourceId = 123;
        const string expectedResourceData = "Resource Data";

        remotingRefServiceMock
            .Setup(x => x.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns(expectedResourceData);

        // Act
        var result = service.GetResourceDataByRefIdAndResourceId(refId, resourceId);

        // Assert
        Assert.Equal(expectedResourceData, result);
        remotingRefServiceMock.Verify(
            x => x.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId.ToString(),
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Once);
    }
}