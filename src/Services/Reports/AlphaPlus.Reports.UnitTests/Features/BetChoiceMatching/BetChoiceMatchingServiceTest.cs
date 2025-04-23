using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.BetChoiceMatching;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;
using NSubstitute;

namespace AlphaPlus.Reports.UnitTests.Features.BetChoiceMatching;

[Trait("Category", "BetChoiceMatching")]
public class BetChoiceMatchingServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly IReportDateService reportDateService;
    private readonly IPService ipService;
    private readonly ICacheRefService cacheRefService;
    private readonly IPartialUMService partialUMService;
    private readonly BetChoiceMatchingService service;

    public BetChoiceMatchingServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        reportDateService = Substitute.For<IReportDateService>();
        ipService = Substitute.For<IPService>();
        cacheRefService = Substitute.For<ICacheRefService>();
        partialUMService = Substitute.For<IPartialUMService>();

        service = new BetChoiceMatchingService(
            repository,
            reportDateService,
            ipService,
            cacheRefService,
            partialUMService);
    }

    [Fact]
    public async Task GetBetChoiceMatching_ShouldReturnCorrectData()
    {
        // Arrange
        var param = new BetChoiceMatchingParam(CustId: 1, WinLossDate: DateTime.Now, BetCount: 10, IsAdminCanViewVNIP: true, SiteName: "site", IsExportExcel: false);
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new BetChoiceMatchingTicketModel { CustId = 1, Username = "user1", Stake = 100, MemberWinLoss = 50, SuperWinLoss = 20, Company = 30, IsRunning = false, BetType = 1, OddsType = 1, Odds = 1.5m, IP = "127.0.0.1" }
        };
        var ips = new List<BetChoiceMatchingSharedIPModel>
        {
            new BetChoiceMatchingSharedIPModel { IP = "127.0.0.1" }
        };

        reportDateService.GetMoveBetsDay().Returns(DateTime.Today.AddDays(-1));
        repository.FetchMultipleAsync<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(Arg.Any<GetBetChoiceMatchingCriteria>())
            .Returns((tickets, ips));
        ipService.IsVNIP(Arg.Any<string>()).Returns(false);

        // Act
        var result = await service.GetBetChoiceMatching(param);

        // Assert
        Assert.Single(result.Item1);
        Assert.Single(result.Item2);
    }

    [Fact]
    public async Task GetBetChoiceMatchingExcel_ShouldReturnFilteredData()
    {
        // Arrange
        var param = new BetChoiceMatchingExcelParam(CustId: 1, WinLossDate: DateTime.Now, BetCount: 10, IsAdminCanViewVNIP: true, SiteName: "site", Username: "user1", Stake: null, MemberWinLossOperator: MemberWinLossOperators.GreatorOrEqual, MemberWinLossValue: null);
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new BetChoiceMatchingTicketModel { CustId = 1, Username = "user1", Stake = 100, MemberWinLoss = 50, SuperWinLoss = 20, Company = 30, IsRunning = false, BetType = 1, OddsType = 1, Odds = 1.5m, IP = "127.0.0.1" }
        };
        var ips = new List<BetChoiceMatchingSharedIPModel>
        {
            new BetChoiceMatchingSharedIPModel { IP = "127.0.0.1" }
        };

        reportDateService.GetMoveBetsDay().Returns(DateTime.Today.AddDays(-1));
        repository.FetchMultipleAsync<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(Arg.Any<GetBetChoiceMatchingCriteria>())
            .Returns((tickets, ips));
        ipService.IsVNIP(Arg.Any<string>()).Returns(false);

        // Act
        var result = await service.GetBetChoiceMatchingExcel(param);

        // Assert
        Assert.Single(result);
        Assert.Equal("user1", result.First().Users.First().Username);
    }

    [Fact]
    public async Task QueryUplines_ShouldReturnCorrectData()
    {
        // Arrange
        var custId = 1;
        var siteName = "site";
        var uplinesModel = new BetChoiceMatchingUplinesModel();
        repository.GetAsync<BetChoiceMatchingUplinesModel>(Arg.Any<BetChoiceUplinesCriteria>()).Returns(uplinesModel);

        // Act
        var result = await service.QueryUplines(custId, siteName);

        // Assert
        Assert.Equal(uplinesModel, result);
    }
}