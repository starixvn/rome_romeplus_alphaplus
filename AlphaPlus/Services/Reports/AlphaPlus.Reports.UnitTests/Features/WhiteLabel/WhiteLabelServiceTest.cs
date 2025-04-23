using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Features.WhiteLabel;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.WhiteLabel;

[Trait("Category", "WhiteLabel")]
public class WhiteLabelServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly WhiteLabelService service;

    public WhiteLabelServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        partialUMServiceMock = new Mock<IPartialUMService>();
        service = new WhiteLabelService(repositoryMock.Object, partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetWhiteLabels_ShouldReturnWhiteLabelList()
    {
        // Arrange
        var custId = 123;
        var siteName = "TestSite";
        var expectedWhiteLabels = new List<IdNameModel>
        {
            new() { Id = 1, Name = "WhiteLabel1" },
            new() { Id = 2, Name = "WhiteLabel2" }
        };

        repositoryMock
            .Setup(r => r.FetchAsync<IdNameModel>(It.IsAny<GetWhiteLabelsCriteria>(), string.Empty))
            .ReturnsAsync(expectedWhiteLabels);

        // Act
        var result = await service.GetWhiteLabels(custId, siteName);

        // Assert
        Assert.Equal(expectedWhiteLabels, result);
        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), siteName), Times.Once);
    }

    [Fact]
    public async Task GetSettlement_ShouldReturnSettlementReportData()
    {
        // Arrange
        var param = new WhiteLabelParam(
            ViewerId: 123,
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31),
            WhiteLabels: "1,2",
            Username: "testUser",
            SiteName: "TestSite"
        );

        var settlementData = new List<SettlementModel>
        {
            new()
            {
                Month = "January 2023",
                WhiteLabel = "Alpha Partners",
                Agent = "AgentOne",
                Master = "MasterA",
                MemberWL = 15000.50m,
                MemberPromotion = 1200.00m,
                LossCarryforward = 0m,
                WhiteLabelWinLoss = 13800.50m,
                WhiteLabelNetWinLoss = 12500.25m,
                WhiteLabelFee = 625.00m,
                WhiteLabelPromotionSettlement = 350.75m,
                WhiteLabelPromotionShare = 180.25m,
                WhiteLabelTotal = 11344.25m,
                MasterEarning = 1200.00m,
                SuperEarning = 600.00m,
                CompanyIncome = 9544.25m,
                CompanyPromotionShare = 669.00m,
                CompanyTotal = 10213.25m
            },
            new()
            {
                Month = "February 2023",
                WhiteLabel = "Beta Gaming",
                Agent = "AgentTwo",
                Master = "MasterB",
                MemberWL = -8500.75m,
                MemberPromotion = 950.00m,
                LossCarryforward = 8500.75m,
                WhiteLabelWinLoss = 0m,
                WhiteLabelNetWinLoss = 0m,
                WhiteLabelFee = 0m,
                WhiteLabelPromotionSettlement = 285.00m,
                WhiteLabelPromotionShare = 142.50m,
                WhiteLabelTotal = 427.50m,
                MasterEarning = 0m,
                SuperEarning = 0m,
                CompanyIncome = 427.50m,
                CompanyPromotionShare = 522.50m,
                CompanyTotal = 950.00m
            }
        };

        repositoryMock
            .Setup(r => r.FetchAsync<SettlementModel>(It.IsAny<GetSettlementCriteria>(), string.Empty))
            .ReturnsAsync(settlementData);

        // Act
        var result = await service.GetSettlement(param);

        // Assert
        Assert.Equal(2, result.DataSource.Count());
        Assert.Equal(6499.75m, result.Total.MemberWL);
        Assert.Equal(2150.00m, result.Total.MemberPromotion);
        Assert.Equal(8500.75m, result.Total.LossCarryforward);
        Assert.Equal(13800.50m, result.Total.WhiteLabelWinLoss);
        Assert.Equal(12500.25m, result.Total.WhiteLabelNetWinLoss);
        Assert.Equal(625.00m, result.Total.WhiteLabelFee);
        Assert.Equal(635.75m, result.Total.WhiteLabelPromotionSettlement);
        Assert.Equal(322.75m, result.Total.WhiteLabelPromotionShare);
        Assert.Equal(11771.75m, result.Total.WhiteLabelTotal);
        Assert.Equal(1200.00m, result.Total.MasterEarning);
        Assert.Equal(600.00m, result.Total.SuperEarning);
        Assert.Equal(9971.75m, result.Total.CompanyIncome);
        Assert.Equal(1191.50m, result.Total.CompanyPromotionShare);
        Assert.Equal(11163.25m, result.Total.CompanyTotal);
        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName), Times.Once);
    }

    [Fact]
    public async Task GetDailyWinLoss_ShouldReturnDailyWinLossReportData()
    {
        // Arrange
        var param = new WhiteLabelParam(
            ViewerId: 123,
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31),
            WhiteLabels: "1,2",
            Username: "testUser",
            SiteName: "TestSite"
        );

        var winlossData = new List<DailyWinLossModel>
        {
            new()
            {
                WinLostDate = DateTime.Today,
                WhiteLabelName = "Alpha Gaming",
                Agent = "Agent001",
                Master = "MasterA",
                MemberWinLoss = 1250.50m,
                Promotion = 150.25m
            },
            new()
            {
                WinLostDate = DateTime.Today.AddDays(-1),
                WhiteLabelName = "Beta Gaming",
                Agent = "Agent002",
                Master = "MasterB",
                MemberWinLoss = -850.75m,
                Promotion = 275.00m
            }
        };

        repositoryMock
            .Setup(r => r.FetchAsync<DailyWinLossModel>(It.IsAny<GetDailyWinLossCriteria>(), string.Empty))
            .ReturnsAsync(winlossData);

        // Act
        var result = await service.GetDailyWinLoss(param);

        // Assert
        Assert.Equal(2, result.DataSource.Count());
        Assert.Equal(399.75m, result.Total.MemberWinLoss);
        Assert.Equal(425.25m, result.Total.Promotion);
        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName), Times.Once);
    }

    [Fact]
    public async Task GetWhiteLabels_ShouldThrowException_WhenPartialUMServiceThrows()
    {
        // Arrange
        var custId = 123;
        var siteName = "TestSite";
        var expectedException = new InvalidOperationException("Access denied");

        partialUMServiceMock
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), siteName))
            .Throws(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetWhiteLabels(custId, siteName));
        Assert.Same(expectedException, exception);
    }

    [Fact]
    public async Task GetSettlement_ShouldThrowException_WhenPartialUMServiceThrows()
    {
        // Arrange
        var param = new WhiteLabelParam(
            ViewerId: 123,
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31),
            WhiteLabels: "1,2",
            Username: "testUser",
            SiteName: "TestSite"
        );
        var expectedException = new InvalidOperationException("Access denied");

        partialUMServiceMock
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetSettlement(param));
        Assert.Same(expectedException, exception);
    }

    [Fact]
    public async Task GetDailyWinLoss_ShouldThrowException_WhenPartialUMServiceThrows()
    {
        // Arrange
        var param = new WhiteLabelParam(
            ViewerId: 123,
            FromDate: new DateTime(2023, 1, 1),
            ToDate: new DateTime(2023, 1, 31),
            WhiteLabels: "1,2",
            Username: "testUser",
            SiteName: "TestSite"
        );
        var expectedException = new InvalidOperationException("Access denied");

        partialUMServiceMock
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), param.SiteName))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetDailyWinLoss(param));
        Assert.Same(expectedException, exception);
    }
}