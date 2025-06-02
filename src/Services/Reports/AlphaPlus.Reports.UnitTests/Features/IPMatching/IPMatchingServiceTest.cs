using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.IPMatching;

[Trait("Category", "IPMatching")]
public class IPMatchingServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock;
    private readonly Mock<IReportDateService> reportDateServiceMock;
    private readonly Mock<IPartialUMService> partialUMServiceMock;
    private readonly Mock<IPService> ipServiceMock;
    private readonly IPMatchingService service;

    public IPMatchingServiceTest()
    {
        repositoryMock = new Mock<IDynamicRepository>();
        reportDateServiceMock = new Mock<IReportDateService>();
        partialUMServiceMock = new Mock<IPartialUMService>();
        ipServiceMock = new Mock<IPService>();

        service = new IPMatchingService(
            repositoryMock.Object,
            reportDateServiceMock.Object,
            partialUMServiceMock.Object,
            ipServiceMock.Object);
    }

    [Fact]
    public async Task GetIPMatching_ShouldFetchDataAndFilterResults()
    {
        // Arrange
        var param = new IPMatchingParam(
            CustId: 123,
            WinlostDate: new DateTime(2023, 1, 1),
            MemberCount: 10,
            SiteName: "TestSite",
            IsAdminCanViewVNIP: false);

        var moveBetsDay = new DateTime(2023, 1, 15);
        reportDateServiceMock.Setup(x => x.GetMoveBetsDay()).ReturnsAsync(moveBetsDay);

        var rawData = GenerateTestData();

        repositoryMock.Setup(x => x.FetchAsync<IPMatchingModel>(It.IsAny<GetIPMatchingCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        // IP check setup - only the third IP is VN
        ipServiceMock.Setup(x => x.IsVNIP("192.168.1.100")).Returns(false);
        ipServiceMock.Setup(x => x.IsVNIP("203.45.78.12")).Returns(false);
        ipServiceMock.Setup(x => x.IsVNIP("172.16.24.89")).Returns(false);

        // Act
        var result = await service.GetIPMatching(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.DataSource.Count());
        Assert.Equal(45, result.Total.MemberCount);
        Assert.Equal(415, result.Total.BetCount);
        Assert.Equal(4081.50m, result.Total.Stake);
        Assert.Equal(-450.95m, result.Total.MemberWinLoss);
        Assert.Equal(205.60m, result.Total.SuperWinLoss);
        Assert.Equal(245.35m, result.Total.Company);

        partialUMServiceMock.Verify(
            x => x.ThrowIfUM(It.IsAny<string>(), param.SiteName),
            Times.Once);
    }

    [Fact]
    public async Task GetIPMatching_ShouldIncludeVNIPs_WhenAdminCanViewVNIP()
    {
        // Arrange
        var param = new IPMatchingParam(
            CustId: 123,
            WinlostDate: new DateTime(2023, 1, 1),
            MemberCount: 10,
            SiteName: "TestSite",
            IsAdminCanViewVNIP: true);

        var moveBetsDay = new DateTime(2023, 1, 15);
        reportDateServiceMock.Setup(x => x.GetMoveBetsDay()).ReturnsAsync(moveBetsDay);

        var rawData = GenerateTestData();

        repositoryMock.Setup(x => x.FetchAsync<IPMatchingModel>(It.IsAny<GetIPMatchingCriteria>(), string.Empty))
            .ReturnsAsync(rawData);

        // IP check setup - only the third IP is VN
        ipServiceMock.Setup(x => x.IsVNIP("192.168.1.100")).Returns(false);
        ipServiceMock.Setup(x => x.IsVNIP("203.45.78.12")).Returns(false);
        ipServiceMock.Setup(x => x.IsVNIP("172.16.24.89")).Returns(true);

        // Act
        var result = await service.GetIPMatching(param);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.TotalRecords);
        Assert.True(result.HasData);
        Assert.True(result.MoreThanOne);
    }

    [Fact]
    public void FilterIPVN_ShouldFilterVNIPs_WhenAdminCannotViewVNIP()
    {
        // Arrange
        var rawData = GenerateTestData();

        // IP check setup - only the third IP is VN
        ipServiceMock.Setup(x => x.IsVNIP("192.168.1.100")).Returns(false);
        ipServiceMock.Setup(x => x.IsVNIP("203.45.78.12")).Returns(true);
        ipServiceMock.Setup(x => x.IsVNIP("172.16.24.89")).Returns(true);

        // Act
        var result = service.FilterIPVN(false, rawData);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalRecords);
        Assert.Equal(15, result.Total.MemberCount);
        Assert.Equal(124, result.Total.BetCount);
        Assert.Equal(1250.50m, result.Total.Stake);
        Assert.Equal(-320.75m, result.Total.MemberWinLoss);
        Assert.Equal(175.50m, result.Total.SuperWinLoss);
        Assert.Equal(145.25m, result.Total.Company);
    }

    [Fact]
    public void FilterIPVN_ShouldNotFilterVNIPs_WhenAdminCanViewVNIP()
    {
        // Arrange
        var rawData = GenerateTestData();

        // IP check setup - only the third IP is VN
        ipServiceMock.Setup(x => x.IsVNIP("192.168.1.100")).Returns(false);
        ipServiceMock.Setup(x => x.IsVNIP("203.45.78.12")).Returns(false);
        ipServiceMock.Setup(x => x.IsVNIP("172.16.24.89")).Returns(true);

        // Act
        var result = service.FilterIPVN(true, rawData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.DataSource.Count());
        Assert.Equal(45, result.Total.MemberCount);
        Assert.Equal(415, result.Total.BetCount);
        Assert.Equal(4081.50m, result.Total.Stake);
        Assert.Equal(-450.95m, result.Total.MemberWinLoss);
        Assert.Equal(205.60m, result.Total.SuperWinLoss);
        Assert.Equal(245.35m, result.Total.Company);
    }

    private static IEnumerable<IPMatchingModel> GenerateTestData()
        => new List<IPMatchingModel>
        {
            new()
            {
                IP = "192.168.1.10000",
                MemberCount = 15,
                BetCount = 124,
                Stake = 1250.50m,
                MemberWinLoss = -320.75m,
                SuperWinLoss = 175.50m,
                Company = 145.25m
            },
            new()
            {
                IP = "203.45.78.12",
                MemberCount = 8,
                BetCount = 87,
                Stake = 980.25m,
                MemberWinLoss = 450.30m,
                SuperWinLoss = -290.15m,
                Company = -160.15m
            },
            new()
            {
                IP = "172.16.24.89",
                MemberCount = 22,
                BetCount = 204,
                Stake = 1850.75m,
                MemberWinLoss = -580.50m,
                SuperWinLoss = 320.25m,
                Company = 260.25m
            }
        };
}