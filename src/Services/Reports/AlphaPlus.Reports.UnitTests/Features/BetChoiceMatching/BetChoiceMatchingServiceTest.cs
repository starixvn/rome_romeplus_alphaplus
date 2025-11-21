using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.BetChoiceMatching;
using AlphaPlus.Reports.Features.Shared;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.UnitTests.Features.BetChoiceMatching;

[Trait("Category", "BetChoiceMatching")]
public class BetChoiceMatchingServiceTest
{
    private readonly Mock<IDynamicRepository> repositoryMock = new();
    private readonly Mock<IReportDateService> reportDateServiceMock = new();
    private readonly Mock<IPService> ipServiceMock = new();
    private readonly Mock<ICacheRefService> cacheRefServiceMock = new();
    private readonly Mock<IPartialUMService> partialUMServiceMock = new();
    private readonly BetChoiceMatchingService service;

    private readonly BetChoiceMatchingParam testParam = new(
        CustId: 123,
        WinLossDate: new DateTime(2023, 1, 1),
        BetCount: 10,
        IsAdminCanViewVNIP: false,
        SiteName: "TestSite",
        SuperId: 456,
        System: "TestSystem",
        IsExportExcel: false);

    public BetChoiceMatchingServiceTest()
    {
        service = new BetChoiceMatchingService(
            repositoryMock.Object,
            reportDateServiceMock.Object,
            ipServiceMock.Object,
            cacheRefServiceMock.Object,
            partialUMServiceMock.Object);
    }

    [Fact]
    public async Task GetBetChoiceMatching_Should_Call_PartialUMService_With_CorrectSettingKey()
    {
        // Arrange
        SetupMocks();

        // Act
        await service.GetBetChoiceMatching(testParam);

        // Assert
        partialUMServiceMock.Verify(
            s => s.ThrowIfUM(It.IsAny<string>(), testParam.SiteName),
            Times.Once);
    }

    [Fact]
    public async Task GetBetChoiceMatching_Should_Call_Repository_With_CorrectCriteria()
    {
        // Arrange
        var moveBetsDay = new DateTime(2023, 1, 15);
        SetupMocks(moveBetsDay);

        // Act
        await service.GetBetChoiceMatching(testParam);

        // Assert
        repositoryMock.Verify(
            r => r.FetchMultipleAsync<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(
                It.Is<GetBetChoiceMatchingCriteria>(c =>
                    c.CustId == testParam.CustId &&
                    c.WinlostDate == testParam.WinLossDate &&
                    c.MoveBetsDay == moveBetsDay &&
                    c.BetCount == testParam.BetCount &&
                    c.IsExportExcel == testParam.IsExportExcel), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task GetBetChoiceMatching_Should_Calculate_DisplayOdds_Correctly_For_Decimal_OddsType()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new()
            {
                CustId = 1,
                BetType = 1, // Handicap
                OddsType = 1, // Decimal
                Odds = 0.95m,
                IP = "192.168.1.1"
            }
        };

        SetupMocks(tickets: tickets);
        cacheRefServiceMock.Setup(c => c.GetOddsTypeName("1")).Returns("Decimal");

        // Act
        var (events, _) = await service.GetBetChoiceMatching(testParam);

        // Assert
        var eventModel = events.First();
        var user = eventModel.Users.First();
        var ticket = user.Tickets.First();

        Assert.Equal(1.95m, ticket.Odds);
        Assert.Equal("1.95", ticket.DisplayOdds);
        Assert.Equal("Decimal", ticket.DisplayOddsType);
    }

    [Fact]
    public async Task GetBetChoiceMatching_Should_Calculate_DisplayOdds_Correctly_For_US_OddsType()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new()
            {
                CustId = 1,
                BetType = 3, // OverUnder
                OddsType = 5, // US
                Odds = 0.95m,
                IP = "192.168.1.1"
            }
        };

        SetupMocks(tickets: tickets);
        cacheRefServiceMock.Setup(c => c.GetOddsTypeName("5")).Returns("US");

        // Act
        var (events, _) = await service.GetBetChoiceMatching(testParam);

        // Assert
        var eventModel = events.First();
        var user = eventModel.Users.First();
        var ticket = user.Tickets.First();

        Assert.Equal(95m, ticket.Odds);
        Assert.Equal("95", ticket.DisplayOdds);
        Assert.Equal("US", ticket.DisplayOddsType);
    }

    [Fact]
    public async Task GetBetChoiceMatching_Should_Mask_VNIP_When_AdminCannotViewVNIP()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new()
            {
                CustId = 1,
                BetType = 1,
                OddsType = 1,
                Odds = 0.95m,
                IP = "192.168.1.1"
            }
        };

        SetupMocks(tickets: tickets);
        ipServiceMock.Setup(i => i.IsVNIP("192.168.1.1")).Returns(true);

        // Act
        var (events, _) = await service.GetBetChoiceMatching(testParam);

        // Assert
        var eventModel = events.First();
        var user = eventModel.Users.First();
        var ticket = user.Tickets.First();

        Assert.Equal("--", ticket.IP);
    }

    [Fact]
    public async Task GetBetChoiceMatching_Should_Not_Mask_NonVNIP_When_AdminCannotViewVNIP()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new()
            {
                CustId = 1,
                BetType = 1,
                OddsType = 1,
                Odds = 0.95m,
                IP = "192.168.1.1"
            }
        };

        SetupMocks(tickets: tickets);
        ipServiceMock.Setup(i => i.IsVNIP("192.168.1.1")).Returns(false);

        // Act
        var (events, _) = await service.GetBetChoiceMatching(testParam);

        // Assert
        var eventModel = events.First();
        var user = eventModel.Users.First();
        var ticket = user.Tickets.First();

        Assert.Equal("192.168.1.1", ticket.IP);
    }

    [Fact]
    public async Task GetBetChoiceMatchingExcel_Should_Filter_By_Username()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new() { CustId = 1, Username = "user1", Stake = 100 },
            new() { CustId = 2, Username = "user2", Stake = 200 },
            new() { CustId = 3, Username = "user3", Stake = 300 }
        };

        var excelParam = new BetChoiceMatchingExcelParam(
            CustId: 123,
            WinLossDate: new DateTime(2023, 1, 1),
            BetCount: 10,
            IsAdminCanViewVNIP: false,
            SiteName: "TestSite",
            SuperId: 456,
            System: "TestSystem",
            Username: "user1,user3",
            Stake: null,
            MemberWinLossOperator: MemberWinLossOperators.LessOrEqual,
            MemberWinLossValue: null);

        SetupMocks(tickets: tickets);

        // Act
        var result = await service.GetBetChoiceMatchingExcel(excelParam);

        // Assert
        Assert.Single(result); // Should include Event1 for user1 and Event2 for user3
        Assert.Equal("user1", result.First().Users.First().Username);
        Assert.Equal("user3", result.First().Users.Last().Username);
    }

    [Fact]
    public async Task GetBetChoiceMatchingExcel_Should_Filter_By_Stake()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new() { CustId = 1, Username = "user1", Stake = 100 },
            new() { CustId = 2, Username = "user2", Stake = 200 },
            new() { CustId = 3, Username = "user3", Stake = 300 }
        };

        var excelParam = new BetChoiceMatchingExcelParam(
            CustId: 123,
            WinLossDate: new DateTime(2023, 1, 1),
            BetCount: 10,
            IsAdminCanViewVNIP: false,
            SiteName: "TestSite",
            SuperId: 456,
            System: "TestSystem",
            Username: "user2,user3",
            Stake: 200,
            MemberWinLossOperator: MemberWinLossOperators.LessOrEqual,
            MemberWinLossValue: null);

        SetupMocks(tickets: tickets);

        // Act
        var result = await service.GetBetChoiceMatchingExcel(excelParam);

        // Assert
        Assert.Equal(1, result.Count()); // Should include Event1 for user2 and Event2 for user3
        var allUsers = result.SelectMany(e => e.Users).ToList();
        Assert.Equal(2, allUsers.Count);
        Assert.Contains(allUsers, u => u.Username == "user2" && u.Stake == 200);
        Assert.Contains(allUsers, u => u.Username == "user3" && u.Stake == 300);
    }

    [Fact]
    public async Task GetBetChoiceMatchingExcel_Should_Filter_By_MemberWinLoss()
    {
        // Arrange
        var tickets = new List<BetChoiceMatchingTicketModel>
        {
            new() { CustId = 1, Username = "user1", Stake = 100, MemberWinLoss = 50, IsRunning = false },
            new() { CustId = 2, Username = "user2", Stake = 200, MemberWinLoss = 100, IsRunning = false },
            new() { CustId = 3, Username = "user3", Stake = 300, MemberWinLoss = -50, IsRunning = false }
        };

        var excelParam = new BetChoiceMatchingExcelParam(
            CustId: 123,
            WinLossDate: new DateTime(2023, 1, 1),
            BetCount: 10,
            IsAdminCanViewVNIP: false,
            SiteName: "TestSite",
            SuperId: 456,
            System: "TestSystem",
            Username: string.Empty,
            Stake: null,
            MemberWinLossOperator: MemberWinLossOperators.GreatorOrEqual,
            MemberWinLossValue: 50);

        SetupMocks(tickets: tickets);

        // Act
        var result = await service.GetBetChoiceMatchingExcel(excelParam);

        // Assert
        Assert.Single(result); // Should include Event1 only
        var allUsers = result.SelectMany(e => e.Users).ToList();
        Assert.Equal(2, allUsers.Count);
        Assert.Contains(allUsers, u => u.Username == "user1" && u.MemberWinLoss == 50);
        Assert.Contains(allUsers, u => u.Username == "user2" && u.MemberWinLoss == 100);
    }

    [Fact]
    public async Task QueryUplines_Should_Call_PartialUMService_With_CorrectSettingKey()
    {
        // Arrange
        const int custId = 123;
        const string siteName = "TestSite";
        partialUMServiceMock.Setup(s => s.ThrowIfUM(It.IsAny<string>(), siteName)).Returns(Task.CompletedTask);
        repositoryMock
            .Setup(r => r.GetAsync<BetChoiceMatchingUplinesModel>(It.IsAny<BetChoiceUplinesCriteria>(), string.Empty))
            .ReturnsAsync(new BetChoiceMatchingUplinesModel());

        // Act
        await service.QueryUplines(custId, siteName);

        // Assert
        partialUMServiceMock.Verify(s => s.ThrowIfUM(It.IsAny<string>(), siteName), Times.Once);
    }

    [Fact]
    public async Task QueryUplines_Should_Call_Repository_With_CorrectCriteria()
    {
        // Arrange
        const int custId = 123;
        const string siteName = "TestSite";
        partialUMServiceMock.Setup(s => s.ThrowIfUM(It.IsAny<string>(), siteName)).Returns(Task.CompletedTask);
        repositoryMock
            .Setup(r => r.GetAsync<BetChoiceMatchingUplinesModel>(It.IsAny<BetChoiceUplinesCriteria>(), string.Empty))
            .ReturnsAsync(new BetChoiceMatchingUplinesModel());

        // Act
        await service.QueryUplines(custId, siteName);

        // Assert
        repositoryMock.Verify(
            r => r.GetAsync<BetChoiceMatchingUplinesModel>(
                It.Is<BetChoiceUplinesCriteria>(c => c.CustId == custId), string.Empty),
            Times.Once);
    }

    private void SetupMocks(
        DateTime? moveBetsDay = null,
        List<BetChoiceMatchingTicketModel> tickets = null,
        List<BetChoiceMatchingSharedIPModel> ips = null)
    {
        reportDateServiceMock
            .Setup(s => s.GetMoveBetsDay())
            .ReturnsAsync(moveBetsDay ?? new DateTime(2023, 1, 15));

        partialUMServiceMock
            .Setup(s => s.ThrowIfUM(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        repositoryMock
            .Setup(r => r.FetchMultipleAsync<BetChoiceMatchingTicketModel, BetChoiceMatchingSharedIPModel>(It.IsAny<GetBetChoiceMatchingCriteria>(), string.Empty))
            .ReturnsAsync((tickets ?? new List<BetChoiceMatchingTicketModel>(), ips ?? new List<BetChoiceMatchingSharedIPModel>()));
    }
}